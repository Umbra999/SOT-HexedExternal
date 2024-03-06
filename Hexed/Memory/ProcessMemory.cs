using Hexed.Wrappers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Hexed.Memory
{
    internal class ProcessMemory
    {
        public readonly Process Process;
        public readonly IntPtr MainWindow;

        public ProcessMemory(Process proc, IntPtr WindowHandle)
        {
            Process = proc;
            MainWindow = WindowHandle;
        }

        #region Read/Write Memory
        public byte[] ReadByteArray(ulong address, int length)
        {
            if (length < 0) return new byte[0];
            byte[] buffer = new byte[length];

            NativeMethods.ReadProcessMemory(Process.Handle, address, buffer, length, IntPtr.Zero);

            return buffer;
        }

        public void WriteByteArray(ulong address, byte[] data)
        {
            NativeMethods.WriteProcessMemory(Process.Handle, address, data, data.Length, IntPtr.Zero);
        }

        public T Read<T>(ulong address)
        {
            Type type = typeof(T);

            if (type == typeof(string)) return (T)Convert.ChangeType(ReadString(address), type);

            if (type.IsEnum)
            {
                Type underlyingType = type.GetEnumUnderlyingType();
                object value = BytesTo<object>(ReadByteArray(address, TypeCache<T>.Size), underlyingType);
                return (T)Enum.ToObject(type, value);
            }

            return BytesTo<T>(ReadByteArray(address, TypeCache<T>.Size));
        }

        public unsafe T[] ReadArray<T>(ulong address, int length) where T : new()
        {
            int size = TypeCache<T>.Size * length;
            byte[] data = ReadByteArray(address, size);
            T[] array = new T[length];

            fixed (byte* b = data)
            {
                for (int i = 0; i < length; i++)
                {
                    array[i] = Marshal.PtrToStructure<T>((IntPtr)(b + i * TypeCache<T>.Size));
                }
            }
            return array;
        }

        public void Write<T>(ulong address, T data) where T : struct
        {
            WriteByteArray(address, ConvertToBytes(data));
        }

        public unsafe void WriteArray<T>(ulong address, T[] array)
        {
            byte[] buffer = new byte[TypeCache<T>.Size * array.Length];

            fixed (byte* b = buffer)
                for (int i = 0; i < array.Length; i++)
                    Marshal.StructureToPtr(array[i], (IntPtr)(b + i * TypeCache<T>.Size), true);

            WriteByteArray(address, buffer);
        }

        public T ReadPointer<T>(ulong baseAddress, params int[] offsets) where T : struct
        {
            return Read<T>(DereferencePointer(baseAddress, offsets));
        }

        internal void Write(object p, float value)
        {
            throw new NotImplementedException();
        }

        public void WritePointer<T>(ulong baseAddress, T data, params int[] offsets) where T : struct
        {
            Write(DereferencePointer(baseAddress, offsets), data);
        }

        public string ReadString(ulong address, bool unicode = false)
        {
            var encoding = unicode ? Encoding.UTF8 : Encoding.Default;
            var numArray = ReadByteArray(address, 255);
            var str = encoding.GetString(numArray);

            if (str.Contains('\0')) str = str.Substring(0, str.IndexOf('\0'));
            return str;
        }

        public void WriteString(ulong address, string str, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.Default;

            WriteByteArray(address, encoding.GetBytes(str));
        }

        public ulong DereferencePointer(ulong baseAddress, params int[] offsets)
        {
            for (int i = 0; i < offsets.Length - 1; i++)
            {
                baseAddress = Read<ulong>(baseAddress + (ulong)offsets[i]);
            }
            return baseAddress + (ulong)offsets[offsets.Length - 1];
        }

        public ulong GetVirtualFunction(ulong classPointer, int index)
        {
            var table = Read<ulong>(classPointer);
            return Read<ulong>(table + (ulong)(index * TypeCache<IntPtr>.Size));
        }
        #endregion

        #region Advanced
        public RemoteAllocation Allocate(int size)
        {
            return new RemoteAllocation(Process, size);
        }
        public RemoteAllocation Allocate<T>() where T : struct
        {
            return new RemoteAllocation(Process, TypeCache<T>.Size);
        }
        public RemoteAllocation Allocate<T>(T data) where T : struct
        {
            return RemoteAllocation.CreateNew<T>(this, data);
        }

        public bool Execute(IntPtr fn, IntPtr param = default)
        {
            RemoteFunction remoteFn = new(Process, fn);
            return remoteFn.Execute(param);
        }

        public bool Execute<T>(IntPtr fn, T param) where T : struct
        {
            RemoteFunction remoteFn = new(Process, fn);
            return remoteFn.Execute(this, param);
        }

        #endregion

        #region Process Modules
        public bool ModuleExists(string name)
        {
            return Process.Modules.Cast<ProcessModule>().Any(module => module.ModuleName == name);
        }

        public ProcessModule ModuleFromName(string name)
        {
            return Process.Modules.Cast<ProcessModule>().FirstOrDefault(item => item.ModuleName == name);
        }

        public IntPtr ModuleBaseAddress(string name)
        {
            ProcessModule module = ModuleFromName(name);
            if (module == null)
                return IntPtr.Zero;
            return module.BaseAddress;
        }

        public int ModuleMemorySize(string name)
        {
            ProcessModule module = ModuleFromName(name);
            if (module == null)
                return 0;
            return module.ModuleMemorySize;
        }
        #endregion

        #region Statis Methods

        public static unsafe byte[] ConvertToBytes<T>(T obj) where T : struct
        {
            byte[] buffer = new byte[TypeCache<T>.Size];
            fixed (byte* b = buffer)
            {
                Marshal.StructureToPtr(obj, (IntPtr)b, true);
            }
            return buffer;
        }

        public static unsafe T BytesTo<T>(byte[] data, Type underlyingType)
        {
            fixed (byte* b = data)
            {
                object result = Marshal.PtrToStructure((IntPtr)b, underlyingType);
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public static unsafe T BytesTo<T>(byte[] data)
        {
            fixed (byte* b = data)
            {
                return Marshal.PtrToStructure<T>((IntPtr)b);
            }
        }

        public static unsafe T BytesTo<T>(byte[] data, int index) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] tmp = new byte[size];
            Array.Copy(data, index, tmp, 0, size);
            return BytesTo<T>(tmp);
        }

        #endregion

        #region Pattern

        public IntPtr FindPattern(byte[] pattern, string mask)
        {
            var sigScan = new SigScan(Process, Process.MainModule.BaseAddress, Process.MainModule.ModuleMemorySize);
            return sigScan.FindPattern(pattern, mask, 0);
        }

        public IntPtr FindPattern(string pattern)
        {
            return FindPattern(pattern, Process.MainModule.BaseAddress, Process.MainModule.ModuleMemorySize);
        }

        public IntPtr FindPattern(string pattern, IntPtr start, int length)
        {
            SigScan sigScan = new(Process, start, length);
            byte[] arrayOfBytes = pattern.Split(' ').Select(b => b.Contains("?") ? (byte)0 : (byte)Convert.ToInt32(b, 16)).ToArray();
            string strMask = string.Join("", pattern.Split(' ').Select(b => b.Contains("?") ? '?' : 'x'));
            return sigScan.FindPattern(arrayOfBytes, strMask, 0);
        }

        public List<IntPtr> FindPatterns(string pattern)
        {
            SigScan sigScan = new(Process, Process.MainModule.BaseAddress, Process.MainModule.ModuleMemorySize);
            byte[] arrayOfBytes = pattern.Split(' ').Select(b => b.Contains("?") ? (byte)0 : (byte)Convert.ToInt32(b, 16)).ToArray();
            string strMask = string.Join("", pattern.Split(' ').Select(b => b.Contains("?") ? '?' : 'x'));
            return sigScan.FindPatterns(arrayOfBytes, strMask, 0);
        }

        #endregion
    }
}

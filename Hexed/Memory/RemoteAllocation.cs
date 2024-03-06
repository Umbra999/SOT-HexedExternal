using Hexed.Wrappers;
using System.Diagnostics;

namespace Hexed.Memory
{
    internal class RemoteAllocation : IDisposable
    {
        private bool isAllocated = false;

        public Process Process { get; private set; }
        public IntPtr Address { get; private set; }

        public RemoteAllocation(Process process)
        {
            Process = process;
        }

        public RemoteAllocation(Process process, int size)
        {
            Process = process;

            Allocate(size);
        }

        ~RemoteAllocation()
        {
            Dispose(false);
        }

        public bool Allocate(int size)
        {
            if (isAllocated) return false;

            Address = NativeMethods.VirtualAllocEx(Process.Handle, IntPtr.Zero, size, NativeMethods.AllocationType.Commit | NativeMethods.AllocationType.Reserve, NativeMethods.MemoryProtection.ExecuteReadWrite);

            isAllocated = true;
            return true;
        }

        public bool Free()
        {
            if (!isAllocated) return false;

            bool result = NativeMethods.VirtualFreeEx(Process.Handle, Address, 0, NativeMethods.FreeType.Release);

            isAllocated = !result;

            return result;
        }

        public static RemoteAllocation CreateNew<T>(ProcessMemory memory, T data) where T : struct
        {
            RemoteAllocation ralloc = new(memory.Process);

            if (!ralloc.Allocate(TypeCache<T>.Size)) return null;

            memory.Write((ulong)ralloc.Address, data);

            return ralloc;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Free();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

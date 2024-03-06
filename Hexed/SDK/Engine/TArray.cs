using Hexed.Core;
using System.Runtime.InteropServices;

namespace Hexed.SDK.Engine
{
    internal class TArray<T> : UObject
    {
        public TArray(ulong address) : base(address) { }

        public ulong Data
        {
            get
            {
                return GameManager.Memory.Read<ulong>(Address);
            }
        }

        public int Count
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + 8); // replace with sizeof?
            }
        }

        public int Max
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + 0xC); // replace with sizeof?
            }
        }

        public unsafe ulong[] GetDataPointer(int Lenght = 0)
        {
            ulong[] data = new ulong[Lenght == 0 ? Count : Lenght];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = GameManager.Memory.Read<ulong>(Data + (ulong)i * 8);
            }

            return data;
        }

        public unsafe ulong[] GetDataAddress(int Lenght = 0)
        {
            ulong[] data = new ulong[Lenght == 0 ? Count : Lenght];

            for (int i = 0; i < data.Length; i++)
            {
                ulong CurrentData = Data + (ulong)i * 8;

                data[i] = CurrentData;
            }

            return data;
        }

        public unsafe T[] GetDataStruct(int Lenght = 0)
        {
            T[] data = new T[Lenght == 0 ? Count : Lenght];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = GameManager.Memory.Read<T>(Data + (ulong)i * (ulong)Marshal.SizeOf<T>());
            }

            return data;
        }

        public unsafe ulong[] GetStructPointer(int length = 0)
        {
            ulong[] data = new ulong[length == 0 ? Count : length];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Data + (ulong)i * (ulong)Marshal.SizeOf<T>();
            }

            return data;
        }
    }
}

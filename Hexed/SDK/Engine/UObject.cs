using Hexed.Core;

namespace Hexed.SDK.Engine
{
    internal class UObject
    {
        public readonly ulong Address;

        public UObject(ulong address) 
        {
            Address = address;
        }

        private string _className;
        public string ClassName
        {
            get
            {
                if (_className == null) _className = UE4Engine.GetName(ClassNameIndex);
                return _className;
            }
        }

        private string _classNameFull; // rework one day
        public string ClassNameFull
        {
            get
            {
                if (_classNameFull == null) _classNameFull = UE4Engine.GetFullName(ClassAddress);
                return _classNameFull;
            }
        }

        public ulong ClassAddress
        {
            get
            {
                return GameManager.Memory.Read<ulong>(Address + 0x10);
            }
        }

        public int ClassNameIndex
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + 0x18);
            }
        }

        public UObject OuterObject
        {
            get
            {
                return new UObject(GameManager.Memory.Read<ulong>(Address + 0x20));
            }
        }

        public UObject ParentObject // might be wrong?
        {
            get
            {
                return new UObject(GameManager.Memory.Read<ulong>(ClassAddress + 0x30));
            }
        }

        public UObject ChildObject // might be wrong?
        {
            get
            {
                return new UObject(GameManager.Memory.Read<ulong>(ClassAddress + 0x48));
            }
        }
    }
}

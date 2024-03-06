using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class UInventoryManipulatorComponent : UActorComponent
    {
        public UInventoryManipulatorComponent(ulong address) : base(address) { }

        public float StashItemTimeout
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UInventoryManipulatorComponent.StashItemTimeout);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UInventoryManipulatorComponent.StashItemTimeout, value);
            }
        }

        public float SlowStashItemTimeout
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UInventoryManipulatorComponent.SlowStashItemTimeout);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UInventoryManipulatorComponent.SlowStashItemTimeout, value);
            }
        }

        public float WieldItemTimeout
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UInventoryManipulatorComponent.WieldItemTimeout);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UInventoryManipulatorComponent.WieldItemTimeout, value);
            }
        }
    }
}

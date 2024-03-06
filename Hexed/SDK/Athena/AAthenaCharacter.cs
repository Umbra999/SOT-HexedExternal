using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class AAthenaCharacter : ACharacter
    {
        public AAthenaCharacter(ulong address) : base(address) { }

        public UHealthComponent HealthComponent
        {
            get
            {
                return new UHealthComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaCharacter.HealthComponent));
            }
        }

        public UWieldedItemComponent WieldedItemComponent
        {
            get
            {
                return new UWieldedItemComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaCharacter.WieldedItemComponent));
            }
        }

        public UInventoryManipulatorComponent InventoryManipulatorComponent
        {
            get
            {
                return new UInventoryManipulatorComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaCharacter.InventoryManipulatorComponent));
            }
        }
    }
}

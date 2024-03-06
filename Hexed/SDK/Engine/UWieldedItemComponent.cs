using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class UWieldedItemComponent : USceneComponent
    {
        public UWieldedItemComponent(ulong address) : base(address) { }

        public AActor CurrentlyWieldedItem
        {
            get
            {
                return new AActor(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UWieldedItemComponent.CurrentlyWieldedItem));
            }
        }
    }
}

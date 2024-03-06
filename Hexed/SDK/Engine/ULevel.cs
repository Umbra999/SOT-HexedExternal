using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class ULevel : UObject
    {
        public ULevel(ulong address) : base(address) { }

        public bool LevelVisibility
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.ULevel.LevelVisibility);
            }
        }

        public UWorld OwningWorld
        {
            get
            {
                return new UWorld(GameManager.Memory.Read<ulong>(Address + ClassOffsets.ULevel.OwningWorld));
            }
        }

        public TArray<AActor> AActors // Undocumented, UE4 always 0xA0
        {
            get
            {
                return new TArray<AActor>(Address + 0xA0);
            }
        }
    }
}

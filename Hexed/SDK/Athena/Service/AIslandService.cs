using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Athena.Structs;

namespace Hexed.SDK.Athena.Service
{
    internal class AIslandService : AActor
    {
        public AIslandService(ulong address) : base(address) { }

        public TArray<FIsland> IslandArray
        {
            get
            {
                return new TArray<FIsland>(Address + ClassOffsets.AIslandService.IslandArray);
            }
        }

        public UIslandDataAsset IslandDataAsset
        {
            get
            {
                return new UIslandDataAsset(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AIslandService.IslandDataAsset));
            }
        }
    }
}

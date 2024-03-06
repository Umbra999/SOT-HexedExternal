using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class UIslandDataAsset : UDataAsset
    {
        public UIslandDataAsset(ulong address) : base(address) { }

        public TArray<UIslandDataAssetEntry> IslandDataEntries
        {
            get
            {
                return new TArray<UIslandDataAssetEntry>(Address + ClassOffsets.UIslandDataAsset.IslandDataEntries);
            }
        }
    }
}

using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena.Service
{
    internal class AKrakenService : AActor
    {
        public AKrakenService(ulong address) : base(address) { }

        public AKraken Kraken
        {
            get
            {
                return new AKraken(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AKrakenService.Kraken));
            }
        }
    }
}

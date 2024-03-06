using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Athena.Structs;

namespace Hexed.SDK.Athena.Service
{
    internal class ACrewService : AActor
    {
        public ACrewService(ulong address) : base(address) { }

        public TArray<FCrew> Crews
        {
            get
            {
                return new TArray<FCrew>(Address + ClassOffsets.ACrewService.Crews);
            }
        }
    }
}

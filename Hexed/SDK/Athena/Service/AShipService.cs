using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Athena.Structs;

namespace Hexed.SDK.Athena.Service
{
    internal class AShipService : AActor
    {
        public AShipService(ulong address) : base(address) { }

        public TArray<FWeakActorHandle> ShipList
        {
            get
            {
                return new TArray<FWeakActorHandle>(Address + ClassOffsets.AShipService.ShipList);
            }
        }

        public TArray<FCrewShipEntry> CrewedShips
        {
            get
            {
                return new TArray<FCrewShipEntry>(Address + ClassOffsets.AShipService.CrewedShips);
            }
        }
    }
}

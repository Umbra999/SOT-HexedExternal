using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Offsets.EnumOffsets;

namespace Hexed.SDK.Athena
{
    internal class AFishingRod : ASkeletalMeshWieldableItem
    {
        public AFishingRod(ulong address) : base(address)  { }

        public EFishingRodServerState ServerState
        {
            get
            {
                return GameManager.Memory.Read<EFishingRodServerState>(Address + ClassOffsets.AFishingRod.ServerState);
            }
        }

        public EFishingRodBattlingState BattlingState
        {
            get
            {
                return GameManager.Memory.Read<EFishingRodBattlingState>(Address + ClassOffsets.AFishingRod.BattlingState);
            }
        }

        public bool IsReeling
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AFishingRod.IsReeling);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AFishingRod.IsReeling, value);
            }
        }
    }
}

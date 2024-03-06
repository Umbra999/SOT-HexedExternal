using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Offsets.EnumOffsets;

namespace Hexed.SDK.Athena
{
    internal class AAthenaPlayerState : APlayerState
    {
        public AAthenaPlayerState(ulong address) : base(address) { }

        public EPlayerActivityType PlayerActivity
        {
            get
            {
                return GameManager.Memory.Read<EPlayerActivityType>(Address + ClassOffsets.AAthenaPlayerState.PlayerActivity);
            }
        }

        public int PlayerIndexOnServer
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + ClassOffsets.AAthenaPlayerState.PlayerIndexOnServer);
            }
        }
    }
}

using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class AOnlineAthenaPlayerController : AAthenaPlayerController
    {
        public AOnlineAthenaPlayerController(ulong address) : base(address) { }

        public bool IdleDisconnectEnabled
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AOnlineAthenaPlayerController.IdleDisconnectEnabled);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AOnlineAthenaPlayerController.IdleDisconnectEnabled, value);
            }
        }
    }
}

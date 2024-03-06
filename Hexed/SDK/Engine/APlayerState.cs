using Hexed.Core;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Engine.Structs;

namespace Hexed.SDK.Engine
{
    internal class APlayerState : AInfo
    {
        public APlayerState(ulong address) : base(address) { }

        public FString PlayerName
        {
            get
            {
                return GameManager.Memory.Read<FString>(Address + ClassOffsets.APlayerState.PlayerName);
            }
        }

        public int PlayerId
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + ClassOffsets.APlayerState.PlayerId);
            }
        }
    }
}

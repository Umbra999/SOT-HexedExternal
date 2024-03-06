using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class UPlayer : UObject
    {
        public UPlayer(ulong address) : base(address) { }

        public APlayerController PlayerController
        {
            get
            {
                return new APlayerController(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UPlayer.PlayerController));
            }
        }
    }
}

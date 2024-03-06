using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class ULocalPlayer : UPlayer
    {
        public ULocalPlayer(ulong address) : base(address) { }

        public UGameViewportClient ViewportClient
        {
            get
            {
                return new UGameViewportClient(GameManager.Memory.Read<ulong>(Address + ClassOffsets.ULocalPlayer.ViewportClient));
            }
        }

        public APlayerController PendingLevelPlayerControllerClass
        {
            get
            {
                return new APlayerController(GameManager.Memory.Read<ulong>(Address + ClassOffsets.ULocalPlayer.PendingLevelPlayerControllerClass));
            }
        }
    }
}

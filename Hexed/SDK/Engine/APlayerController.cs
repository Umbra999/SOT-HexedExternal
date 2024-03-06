using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class APlayerController : AController
    {
        public APlayerController(ulong address) : base(address) { }

        public APawn AcknowledgedPawn
        {
            get
            {
                return new APawn(GameManager.Memory.Read<ulong>(Address + ClassOffsets.APlayerController.AcknowledgedPawn));
            }
        }

        public UPlayer Player
        {
            get
            {
                return new UPlayer(GameManager.Memory.Read<ulong>(Address + ClassOffsets.APlayerController.Player));
            }
        }

        public APlayerCameraManager PlayerCameraManager
        {
            get
            {
                return new APlayerCameraManager(GameManager.Memory.Read<ulong>(Address + ClassOffsets.APlayerController.PlayerCameraManager));
            }
        }

        public APlayerCameraManager PlayerCameraManagerClass
        {
            get
            {
                return new APlayerCameraManager(GameManager.Memory.Read<ulong>(Address + ClassOffsets.APlayerController.PlayerCameraManagerClass));
            }
        }
    }
}

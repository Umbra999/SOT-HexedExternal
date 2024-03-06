using Hexed.Core;
using Hexed.Extensions;

namespace Hexed.Modules
{
    internal class AntiAFK
    {
        public static void OnConnected()
        {
            if (ConfigHandler.AntiAFK) EnableAntiAFK();
            else DisableAntiAFK();
        }

        public static void EnableAntiAFK()
        {
            if (GameManager.OnlinePlayerController == null) return;

            if (GameManager.OnlinePlayerController.IdleDisconnectEnabled) GameManager.OnlinePlayerController.IdleDisconnectEnabled = false;
        }

        public static void DisableAntiAFK()
        {
            if (GameManager.OnlinePlayerController == null) return;

            if (!GameManager.OnlinePlayerController.IdleDisconnectEnabled) GameManager.OnlinePlayerController.IdleDisconnectEnabled = true;
        }
    }
}

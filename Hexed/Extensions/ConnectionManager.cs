using Hexed.Core;

namespace Hexed.Extensions
{
    internal class ConnectionManager
    {
        private static int LastPlayerId = 0;

        public static bool IsServerSwitched()
        {
            if (LastPlayerId != GameManager.OnlinePlayerController.AcknowledgedPawn.PlayerState.PlayerId)
            {
                LastPlayerId = GameManager.OnlinePlayerController.AcknowledgedPawn.PlayerState.PlayerId;
                return true;
            }

            return false;
        }

        public static bool IsInWorld()
        {
            return GameManager.CrewList.Count > 0 && GameManager.IslandList.Count > 0 && GameManager.OnlinePlayerController.AcknowledgedPawn.PlayerState.PlayerId != 0;
        }
    }
}

using Hexed.Core;
using Hexed.SDK.Athena;
using System.Numerics;

namespace Hexed.Wrappers
{
    internal class GameHelper
    {
        public static float GetDistance(Vector3 Position)
        {
            return Vector3.Distance(GameManager.LocalPlayer.PlayerController.AcknowledgedPawn.RootComponent.Transform.Translation, Position);
        }

        public static int GetDistanceInMeter(Vector3 Position)
        {
            return (int)(Vector3.Distance(GameManager.LocalPlayer.PlayerController.AcknowledgedPawn.RootComponent.Transform.Translation, Position) * 0.01f);
        }

        public static int GetDistanceInMeter(Vector3 Root, Vector3 Position)
        {
            return (int)(Vector3.Distance(Root, Position) * 0.01f);
        }

        public static AAthenaPlayerCharacter GetLocalPlayerCharacter()
        {
            foreach (AAthenaPlayerCharacter Pirate in GameManager.PirateList)
            {
                if (Pirate.PlayerState.PlayerId == GameManager.OnlinePlayerController.AcknowledgedPawn.PlayerState.PlayerId) return Pirate;
            }

            foreach (AAthenaPlayerCharacter Pirate in GameManager.GhostPirateList)
            {
                if (Pirate.PlayerState.PlayerId == GameManager.OnlinePlayerController.AcknowledgedPawn.PlayerState.PlayerId) return Pirate;
            }

            return null;
        }

        public static string GetPlayerActivity(SDK.Offsets.EnumOffsets.EPlayerActivityType Activity)
        {
            switch (Activity)
            {
                case SDK.Offsets.EnumOffsets.EPlayerActivityType.None:
                    return "None";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Bailing:
                    return "Bailing";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Cannon:
                    return "Cannon";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Capstan:
                    return "Capstan";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.CarryingBooty:
                    return "Carrying Booty";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Dead:
                    return "Dead";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Digging:
                    return "Digging";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Dousing:
                    return "Dousing";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.EmptyingBucket:
                    return "Emptying Bucket";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Harpoon:
                    return "Harpoon";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.LoseHealth:
                    return "Lose Health";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Repairing:
                    return "Repairing";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Sails:
                    return "Sails";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.UndoingRepair:
                    return "Undoing Repair";

                case SDK.Offsets.EnumOffsets.EPlayerActivityType.Wheel:
                    return "Wheel";
            }

            return "None";
        }
    }
}

using Hexed.Extensions;
using Hexed.SDK.Athena;
using Hexed.Wrappers;

namespace Hexed.Modules
{
    internal class FastLadder
    {
        public static void Update()
        {
            if (!ConfigHandler.FastLadder) return;

            AAthenaPlayerCharacter Pirate = GameHelper.GetLocalPlayerCharacter();
            if (Pirate == null) return;

            UClimbingComponent ClimbingComponent = Pirate.ClimbingComponent;
            if (ClimbingComponent == null) return;

            // add check if player is on ladder

            if (GeneralHelper.IsKeyDown(0x57))
            {
                if (ClimbingComponent.ServerHeight != 9999) ClimbingComponent.ServerHeight = 9999;
            }

            else if (GeneralHelper.IsKeyDown(0x53))
            {
                if (ClimbingComponent.ServerHeight != 0) ClimbingComponent.ServerHeight = 0;
            }
        }
    }
}

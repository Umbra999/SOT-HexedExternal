using Hexed.Extensions;
using Hexed.SDK.Athena;
using Hexed.Wrappers;

namespace Hexed.Modules
{
    internal class AntiFreeze
    {
        public static void Update()
        {
            if (!ConfigHandler.AntiFreeze) return;

            AAthenaPlayerCharacter Pirate = GameHelper.GetLocalPlayerCharacter();
            if (Pirate == null) return;

            if (Pirate.PreventJumping)
            {
                Pirate.PreventJumping = false;
                Logger.Log("Intercepted Jump prevention");
            }

            if (Pirate.StopMovementAndPreventSwimming)
            {
                Pirate.StopMovementAndPreventSwimming = false;
                Logger.Log("Intercepted Movement prevention");
            }
        }
    }
}

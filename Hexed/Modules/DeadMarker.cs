using Hexed.SDK.Athena;
using Hexed.Wrappers;
using System.Numerics;

namespace Hexed.Modules
{
    internal class DeadMarker
    {
        public static Vector3 LastDeadPosition = Vector3.Zero;

        public static void Reset()
        {
            LastDeadPosition = Vector3.Zero;
        }

        public static void Update()
        {
            AAthenaPlayerCharacter Pirate = GameHelper.GetLocalPlayerCharacter();
            if (Pirate == null) return;

            if (Pirate.HealthComponent.CurrentHealthInfo.Health == 0) LastDeadPosition = Pirate.RootComponent.Transform.Translation;
        }
    }
}

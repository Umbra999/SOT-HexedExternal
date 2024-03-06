using Hexed.Extensions;
using Hexed.SDK.Athena;
using Hexed.Wrappers;
namespace Hexed.Modules
{
    internal class AntiDrunk
    {
        public static void Update()
        {
            if (!ConfigHandler.AntiDrunk) return;

            AAthenaPlayerCharacter Pirate = GameHelper.GetLocalPlayerCharacter();
            if (Pirate == null) return;

            UDrunkennessComponent DrunkennessComponent = Pirate.DrunkennessComponent;
            if (DrunkennessComponent == null) return;

            if (DrunkennessComponent.CurrentDrunkenness0x02 != 0) DrunkennessComponent.CurrentDrunkenness0x02 = 0;
        }
    }
}

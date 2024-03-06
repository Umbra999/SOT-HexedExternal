using Hexed.Wrappers;

namespace Hexed.Extensions
{
    internal class ConfigHandler
    {
        public static IniFile Ini;

        public static void Init()
        {
            Ini = new IniFile("Config.ini");

            AntiAFK = Ini.GetBool("Toggles", "AntiAFK");
            AutoFish = Ini.GetBool("Toggles", "AutoFish");
            AntiFreeze = Ini.GetBool("Toggles", "AntiFreeze");
            FastLadder = Ini.GetBool("Toggles", "FastLadder");
            AntiDrunk = Ini.GetBool("Toggles", "AntiDrunk");
            DeadMarker = Ini.GetBool("Toggles", "DeadMarker");
        }

        public static bool AntiAFK;
        public static bool AutoFish;
        public static bool AntiFreeze;
        public static bool FastLadder;
        public static bool AntiDrunk;
        public static bool DeadMarker;
    }
}

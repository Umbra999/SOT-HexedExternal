using Hexed.Core;
using Hexed.Modules;
using Hexed.Wrappers;

namespace Hexed.Extensions
{
    internal class ClickGUI
    {
        public static bool isMenuShown = false;
        private static int LastKeyTime = 0;
        public static int ItemIndex = 0;
        public static List<CustomObjects.ToggleState> Toggles = new();

        public static void Init()
        {
            Toggles.Add(new()
            {
                Name = "Anti AFK",
                Toggled = ConfigHandler.AntiAFK,
                OnAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AntiAFK", true);
                    ConfigHandler.AntiAFK = true;
                    AntiAFK.EnableAntiAFK();
                },
                OffAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AntiAFK", false);
                    ConfigHandler.AntiAFK = false;
                    AntiAFK.DisableAntiAFK();
                }
            });

            Toggles.Add(new()
            {
                Name = "Anti Drunk",
                Toggled = ConfigHandler.AntiDrunk,
                OnAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AntiDrunk", true);
                    ConfigHandler.AntiDrunk = true;
                },
                OffAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AntiDrunk", false);
                    ConfigHandler.AntiDrunk = false;
                }
            });

            Toggles.Add(new()
            {
                Name = "Anti Freeze",
                Toggled = ConfigHandler.AntiFreeze,
                OnAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AntiFreeze", true);
                    ConfigHandler.AntiFreeze = true;
                },
                OffAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AntiFreeze", false);
                    ConfigHandler.AntiFreeze = false;
                }
            });

            Toggles.Add(new()
            {
                Name = "Auto Fish",
                Toggled = ConfigHandler.AutoFish,
                OnAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AutoFish", true);
                    ConfigHandler.AutoFish = true;
                },
                OffAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "AutoFish", false);
                    ConfigHandler.AutoFish = false;
                }
            });

            Toggles.Add(new()
            {
                Name = "Fast Ladder",
                Toggled = ConfigHandler.FastLadder,
                OnAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "FastLadder", true);
                    ConfigHandler.FastLadder = true;
                },
                OffAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "FastLadder", false);
                    ConfigHandler.FastLadder = false;
                }
            });

            Toggles.Add(new()
            {
                Name = "Dead Marker",
                Toggled = ConfigHandler.DeadMarker,
                OnAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "DeadMarker", true);
                    ConfigHandler.DeadMarker = true;
                },
                OffAction = () =>
                {
                    ConfigHandler.Ini.SetBool("Toggles", "DeadMarker", false);
                    ConfigHandler.DeadMarker = false;
                }
            });
        }

        public static void Update()
        {
            if (NativeMethods.GetForegroundWindow() != GameManager.Memory.MainWindow) return;

            if (GeneralHelper.IsKeyDown(0x2D) && LastKeyTime < Environment.TickCount - 150)
            {
                LastKeyTime = Environment.TickCount;
                isMenuShown = !isMenuShown;
            }

            if (isMenuShown)
            {
                if (GeneralHelper.IsKeyDown(0x26) && LastKeyTime < Environment.TickCount - 150)
                {
                    if (ItemIndex == 0) return;
                    ItemIndex--;
                    LastKeyTime = Environment.TickCount;
                }

                else if (GeneralHelper.IsKeyDown(0x28) && LastKeyTime < Environment.TickCount - 150)
                {
                    if (ItemIndex == Toggles.Count - 1) return;
                    ItemIndex++;
                    LastKeyTime = Environment.TickCount;
                }

                else if (GeneralHelper.IsKeyDown(0x0D) && LastKeyTime < Environment.TickCount - 150)
                {
                    LastKeyTime = Environment.TickCount;

                    CustomObjects.ToggleState[] toggleKeys = Toggles.ToArray();
                    CustomObjects.ToggleState currentToggle = toggleKeys[ItemIndex];

                    if (currentToggle.Toggled) currentToggle.OffAction();
                    else currentToggle.OnAction();
                    currentToggle.Toggled = !currentToggle.Toggled;

                    Logger.Log($"Toggled {currentToggle.Name} to {currentToggle.Toggled}");
                }
            }
        }
    }
}

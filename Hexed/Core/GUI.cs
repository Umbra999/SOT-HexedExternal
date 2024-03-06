using GameOverlay.Drawing;
using GameOverlay.Windows;
using Hexed.Extensions;
using Hexed.Modules;
using Hexed.SDK.Athena;
using Hexed.SDK.Engine;
using Hexed.Wrappers;
using System.Numerics;

namespace Hexed.Core
{
    internal class GUI
    {
        private static Font Consolas;
        private static SolidBrush TextBackground;
        private static SolidBrush DefaultText;

        private static void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            try
            {
                Graphics gfx = e.Graphics;

                gfx.ClearScene(gfx.CreateSolidBrush(0, 0, 0, 0));

                if (NativeMethods.GetForegroundWindow() != GameManager.Memory.MainWindow || !ConnectionManager.IsInWorld()) return;

                gfx.DrawCrosshair(DefaultText, gfx.Width / 2, gfx.Height / 2, 4, 1, CrosshairStyle.Cross);

                DrawStats(gfx);
                DrawCompass(gfx);
                DrawCrews(gfx);
                DrawUnknownCrews(gfx);
                DrawKraken(gfx);
                DrawMermaid(gfx);
                DrawClouds(gfx);
                DrawStorm(gfx);
                DrawShark(gfx);
                DrawTreasure(gfx);
                DrawDrowning(gfx);
                DrawDeadMarker(gfx);
                DrawClickGUI(gfx);
                //DrawDebugActors(gfx);
            }
            catch (Exception ex) { Logger.LogError(ex); }
        }

        private static void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            Graphics gfx = e.Graphics;

            if (e.RecreateResources) return;

            Consolas = gfx.CreateFont("Consolas", 14);
            TextBackground = gfx.CreateSolidBrush(0, 0, 0, 90);
            DefaultText = gfx.CreateSolidBrush(255, 255, 255);
        }

        public static void Run()
        {
            Graphics gfx = new()
            {
                //VSync = true,
                UseMultiThreadedFactories = true,           
            };

            StickyWindow _window = new(GameManager.Memory.MainWindow, gfx)
            {
                IsTopmost = true,
                IsVisible = true,
                Title = "Hexed",
                MenuName = "Hexed",
                ClassName = "Hexed",
                ParentWindowHandle = GameManager.Memory.MainWindow, 
            };

            _window.DrawGraphics += _window_DrawGraphics;
            _window.SetupGraphics += _window_SetupGraphics;

            _window.Create();
        }

        private static void DrawStats(Graphics gfx)
        {
            gfx.DrawTextWithBackground(Consolas, DefaultText, TextBackground, 10, gfx.Height - 20, $"Crews: {GameManager.CrewList.Count} | Players: {GameManager.CrewList.Values.Sum(arr => arr.Length)} | Islands: {GameManager.IslandList.Count}");
        }

        private static void DrawCompass(Graphics gfx)
        {
            Quaternion Rotation = GameManager.LocalPlayer.PlayerController.PlayerCameraManager.RootComponent.Transform.Rotation;
            string rotationAngle = Wrappers.Math.GetDirection(Wrappers.Math.GetRotationAngle(Rotation.X, Rotation.Y, Rotation.Z, Rotation.W));
            Point MeasuredString = gfx.MeasureString(Consolas, rotationAngle);
            gfx.DrawText(Consolas, 17, DefaultText, gfx.Width / 2 - (MeasuredString.X / 2), 20, rotationAngle);
        }

        private static void DrawUnknownCrews(Graphics gfx)
        {
            foreach (CustomObjects.UndefinedCrew Ship in CrewManager.GetUndefinedCrews())
            {
                string ShipEntry = Ship.ShipType.ToString().ToUpper() + " (FAR)";

                int Distance = GameHelper.GetDistanceInMeter(Ship.ShipProxy.RootComponent.Transform.Translation);

                ShipEntry += $" | {Distance}m";

                Point MeasuredString = gfx.MeasureString(Consolas, ShipEntry);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Ship.ShipProxy.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(190, 190, 255), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, ShipEntry);
            }
        }

        private static void DrawCrews(Graphics gfx)
        {
            int itemOffset = 22;
            int startOffset = 85;

            foreach (CustomObjects.DefinedCrew crew in CrewManager.GetDefinedCrews())
            {
                System.Drawing.Color crewColor = Wrappers.Math.ColorByGuid(crew.CrewId);
                SolidBrush crewBrush = gfx.CreateSolidBrush(crewColor.R, crewColor.G, crewColor.B);

                string crewEntry = crew.ShipType.ToString().ToUpper();

                if (crew.CurrentShip != null)
                {
                    int ShipDistance = GameHelper.GetDistanceInMeter(crew.CurrentShip.RootComponent.Transform.Translation);

                    crewEntry += $" | {ShipDistance}m";

                    Point MeasuredString = gfx.MeasureString(Consolas, crewEntry);
                    Vector2 ShipScreenLocation = Wrappers.Math.WorldToScreen(crew.CurrentShip.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                    gfx.DrawTextWithBackground(Consolas, crewBrush, TextBackground, ShipScreenLocation.X - (MeasuredString.X / 2), ShipScreenLocation.Y, crewEntry);
                }

                crewEntry = "[" + crewEntry + "]\n";

                foreach (AAthenaPlayerState player in crew.Players)
                {
                    string playerEntry = player.PlayerName.ToString() == "" ? $"PLAYER {player.PlayerIndexOnServer}" : player.PlayerName.ToString();

                    if (player.PlayerActivity != SDK.Offsets.EnumOffsets.EPlayerActivityType.None) playerEntry += $" [{GameHelper.GetPlayerActivity(player.PlayerActivity)}]";

                    AAthenaPlayerCharacter playerPirate = GameManager.PirateList.FirstOrDefault(x => new AAthenaPlayerState(x.PlayerState.Address).PlayerIndexOnServer == player.PlayerIndexOnServer) ?? GameManager.GhostPirateList.FirstOrDefault(x => new AAthenaPlayerState(x.PlayerState.Address).PlayerIndexOnServer == player.PlayerIndexOnServer);
                    if (playerPirate != null && playerPirate.Address != GameHelper.GetLocalPlayerCharacter()?.Address)
                    {
                        int playerDistance = GameHelper.GetDistanceInMeter(playerPirate.RootComponent.Transform.Translation);
                        playerEntry += $" | {playerDistance}m";

                        if (playerDistance < 300) // hardcoded limit to prevent players in 1000m showing on ships
                        {
                            Point MeasuredString = gfx.MeasureString(Consolas, playerEntry);
                            Vector2 PlayerScreenLocation = Wrappers.Math.WorldToScreen(playerPirate.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                            gfx.DrawTextWithBackground(Consolas, crewBrush, TextBackground, PlayerScreenLocation.X - (MeasuredString.X / 2), PlayerScreenLocation.Y, playerEntry);

                            float progressBarWidth = 110;
                            float progressBarHeight = 6;
                            float progressBarStroke = 1;
                            float progressBarLeft = PlayerScreenLocation.X - (progressBarWidth / 2);
                            float progressBarTop = PlayerScreenLocation.Y + MeasuredString.Y + 5;

                            gfx.DrawVerticalProgressBar(DefaultText, gfx.CreateSolidBrush(40, 255, 40), progressBarLeft, progressBarTop, progressBarLeft + progressBarWidth, progressBarTop + progressBarHeight, progressBarStroke, playerPirate.HealthComponent.CurrentHealthInfo.Health);
                        }

                        playerEntry += $" | {(int)playerPirate.HealthComponent.CurrentHealthInfo.Health}HP";
                    }

                    crewEntry += playerEntry + "\n";
                }

                gfx.DrawText(Consolas, 15, crewBrush, 30, startOffset, crewEntry);

                startOffset += ((crew.Players.Length + 1) * itemOffset) + 7;
            }
        }

        private static void DrawKraken(Graphics gfx)
        {
            if (GameManager.Kraken != null && GameManager.Kraken.RootComponent.Transform.Translation != Vector3.Zero)
            {          
                int Distance = GameHelper.GetDistanceInMeter(GameManager.Kraken.RootComponent.Transform.Translation);
                string KrakenEntry = $"Kraken | {Distance}m";
                Point MeasuredString = gfx.MeasureString(Consolas, KrakenEntry);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(GameManager.Kraken.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(105, 50, 168), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, KrakenEntry);
            }
        }

        private static void DrawMermaid(Graphics gfx)
        {
            foreach (AMermaid Mermaid in GameManager.MermaidList)
            {
                int Distance = GameHelper.GetDistanceInMeter(Mermaid.RootComponent.Transform.Translation);
                string MermaidEntry = $"Mermaid | {Distance}m";
                Point MeasuredString = gfx.MeasureString(Consolas, MermaidEntry);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Mermaid.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(115, 255, 115), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, MermaidEntry);
            }
        }

        private static void DrawClouds(Graphics gfx)
        {
            foreach (var Fort in GameManager.FortList)
            {
                int Distance = GameHelper.GetDistanceInMeter(Fort.Key.RootComponent.Transform.Translation);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Fort.Key.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));

                string Entry = "";
                switch (Fort.Value)
                {
                    case CustomObjects.FortType.SkeletonFort:
                        Entry = "Skeleton Fort";
                        break;

                    case CustomObjects.FortType.FortuneFort:
                        Entry = "Fort of Fortune";
                        break;

                    case CustomObjects.FortType.SkeletonFleet:
                        Entry = "Skeleton Fleet";
                        break;

                    case CustomObjects.FortType.GhostFleet:
                        Entry = "Ghost Fleet";
                        break;

                    case CustomObjects.FortType.AshenWinds:
                        Entry = "Ashen Winds";
                        break;

                    case CustomObjects.FortType.DamnedFort:
                        Entry = "Fort of the Damned";
                        break;
                }

                Entry += $" | {Distance}m";

                Point MeasuredString = gfx.MeasureString(Consolas, Entry);
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(212, 4, 136), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, Entry);
            }
        }

        private static void DrawStorm(Graphics gfx)
        {
            foreach (AStorm Storm in GameManager.StormList)
            {
                int Distance = GameHelper.GetDistanceInMeter(Storm.RootComponent.Transform.Translation);
                string StormEntry = $"Storm | {Distance}m";
                Point MeasuredString = gfx.MeasureString(Consolas, StormEntry);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Storm.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(190, 190, 190), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, StormEntry);
            }
        }

        private static void DrawShark(Graphics gfx)
        {
            foreach (AActor Shark in GameManager.SharkList)
            {
                int Distance = GameHelper.GetDistanceInMeter(Shark.RootComponent.Transform.Translation);
                string SharkEntry = $"Shark | {Distance}m";
                Point MeasuredString = gfx.MeasureString(Consolas, SharkEntry);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Shark.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(102, 166, 255), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, SharkEntry);
            }

            foreach (AActor Megalodon in GameManager.MegalodonList)
            {
                int Distance = GameHelper.GetDistanceInMeter(Megalodon.RootComponent.Transform.Translation);
                string MegalodonEntry = $"Megalodon | {Distance}m";
                Point MeasuredString = gfx.MeasureString(Consolas, MegalodonEntry);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Megalodon.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(102, 166, 255), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, MegalodonEntry);
            }
        }

        private static void DrawTreasure(Graphics gfx)
        {
            foreach (var Treasure in GameManager.TreasureList)
            {
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Treasure.Key.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));

                System.Drawing.Color Rarity = System.Drawing.Color.White;
                switch (Treasure.Value)
                {
                    case CustomObjects.TreasureRarity.Common:
                        Rarity = System.Drawing.Color.SaddleBrown;
                        break;

                    case CustomObjects.TreasureRarity.Danger:
                        Rarity = System.Drawing.Color.DarkRed;
                        break;

                    case CustomObjects.TreasureRarity.Rare:
                        Rarity = System.Drawing.Color.LightGreen;
                        break;

                    case CustomObjects.TreasureRarity.Mythic:
                        Rarity = System.Drawing.Color.Green;
                        break;

                    case CustomObjects.TreasureRarity.Legendary:
                        Rarity = System.Drawing.Color.DarkBlue;
                        break;

                    case CustomObjects.TreasureRarity.Special:
                        Rarity = System.Drawing.Color.DeepPink;
                        break;
                }

                gfx.DrawCircle(gfx.CreateSolidBrush(Rarity.R, Rarity.G, Rarity.B), ScreenLocation.X, ScreenLocation.Y, 4, 1);               
            }
        }

        private static void DrawDrowning(Graphics gfx)
        {
            foreach (AAthenaPlayerCharacter Player in GameManager.PirateList)
            {
                int PlayerDistance = GameHelper.GetDistanceInMeter(Player.RootComponent.Transform.Translation);
                if (PlayerDistance == 0 && Player.DrowningComponent.OxygenLevel < 1f)
                {
                    float progressBarWidth = 260;
                    float progressBarHeight = 12;
                    float progressBarStroke = 1;
                    float progressBarLeft = (gfx.Width - progressBarWidth) / 2;
                    float progressBarTop = gfx.Height - 30;

                    gfx.DrawVerticalProgressBar(DefaultText, gfx.CreateSolidBrush(0, 166, 255), progressBarLeft, progressBarTop, progressBarLeft + progressBarWidth, progressBarTop + progressBarHeight, progressBarStroke, Player.DrowningComponent.OxygenLevel * 100);
                    break;
                }
            }
        }

        private static void DrawDeadMarker(Graphics gfx)
        {
            if (!ConfigHandler.DeadMarker || DeadMarker.LastDeadPosition == Vector3.Zero) return;

            int Distance = GameHelper.GetDistanceInMeter(DeadMarker.LastDeadPosition);
            string StormEntry = $"Last Dead | {Distance}m";
            Point MeasuredString = gfx.MeasureString(Consolas, StormEntry);
            Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(DeadMarker.LastDeadPosition, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
            gfx.DrawTextWithBackground(Consolas, gfx.CreateSolidBrush(190, 190, 190), TextBackground, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, StormEntry);
        }

        private static void DrawClickGUI(Graphics gfx)
        {
            if (!ClickGUI.isMenuShown) return;

            int startIndex = 80;

            string Title = $"H E X E D";
            Point MeasuredString = gfx.MeasureString(Consolas, Title);

            gfx.DrawText(Consolas, 16, gfx.CreateSolidBrush(149, 0, 255), gfx.Width - 300 + (MeasuredString.X / 2), startIndex - 5, "HEXED");

            CustomObjects.ToggleState[] toggleKeys = ClickGUI.Toggles.ToArray();
            CustomObjects.ToggleState currentToggle = toggleKeys[ClickGUI.ItemIndex];

            foreach (CustomObjects.ToggleState Toggle in ClickGUI.Toggles)
            {
                startIndex += 20;
                gfx.DrawText(Consolas, 15, Toggle.Name == currentToggle.Name ? gfx.CreateSolidBrush(149, 0, 255) : gfx.CreateSolidBrush(255, 255, 255), gfx.Width - 300, startIndex, $"{Toggle.Name} [{(Toggle.Toggled ? "ON" : "OFF")}]");
            }
        }

        private static void DrawDebugActors(Graphics gfx)
        {
            foreach (AActor Actor in GameManager.ActorList)
            {
                Point MeasuredString = gfx.MeasureString(Consolas, Actor.ClassName);
                Vector2 ScreenLocation = Wrappers.Math.WorldToScreen(Actor.RootComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Translation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.TransformComponent.Transform.Rotation, GameManager.LocalPlayer.PlayerController.PlayerCameraManager.ViewTarget.POV.FOV, new Vector2(gfx.Width, gfx.Height));
                gfx.DrawText(Consolas, DefaultText, ScreenLocation.X - (MeasuredString.X / 2), ScreenLocation.Y, Actor.ClassName);
            }
        }
    }
}

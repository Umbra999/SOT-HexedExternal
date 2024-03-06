using Hexed.Core;
using Hexed.Extensions;
using Hexed.HexedServer;
using Hexed.Memory;
using Hexed.Modules;
using Hexed.SDK.Athena;
using Hexed.SDK.Athena.Service;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using Hexed.Wrappers;
using System.Diagnostics;
using static Hexed.SDK.Athena.Structs;
using static Hexed.Wrappers.CustomObjects;

namespace Hexed
{
    internal class Boot
    {
        public sealed class HexedEntry : Attribute { }

        [HexedEntry]
        public static void Main(string[] args)
        {
            //if (args.Length != 1) return;

            //ServerHandler.Init(args[0]);

            Process SotProc = GeneralHelper.GetProcessByName("SoTGame");
            if (SotProc == null)
            {
                Logger.LogError("Sea of Thieves is not running");
                Thread.Sleep(-1);
            }

            IntPtr WindowHandle = SotProc.MainWindowHandle == IntPtr.Zero ? NativeMethods.FindWindow(null, "Sea of Thieves") : SotProc.MainWindowHandle;
            if (WindowHandle == IntPtr.Zero)
            {
                Logger.LogError("Failed to find Window");
                Thread.Sleep(-1);
            }

            GameManager.Memory = new ProcessMemory(SotProc, WindowHandle);
            Logger.Log($"Attached to {GameManager.Memory.Process.ProcessName} [{GameManager.Memory.Process.Id}]");
            Logger.LogDebug($"Hooked Module {GameManager.Memory.Process.MainModule.ModuleName} [{GameManager.Memory.Process.MainModule.BaseAddress:X}] | {Wrappers.Math.ByteSizeToString(GameManager.Memory.Process.MainModule.ModuleMemorySize)}");

            SigManager.FindPatterns();

            Logger.LogDebug($"GWorld Address: {SigManager.UWorldAddress}");
            Logger.LogDebug($"GObjects Address: {SigManager.GObjectsAddress}");
            Logger.LogDebug($"GNames Address: {SigManager.GNamesAddress}");

            ConfigHandler.Init();
            ClickGUI.Init();

            GUI.Run();

            while (!SotProc.HasExited)
            {
                DoLoops();
                Thread.Sleep(1);
            }
        }
        private static void DoLoops()
        {
            MainLoop();

            if (!ConnectionManager.IsInWorld()) return;

            if (ConnectionManager.IsServerSwitched())
            {
                DeadMarker.Reset();
                AntiAFK.OnConnected();
            }

            ClickGUI.Update();
            AutoFish.Update();
            AntiFreeze.Update();
            FastLadder.Update();
            AntiDrunk.Update();
            DeadMarker.Update();
        }

        private static void MainLoop()
        {
            List<AActor> ActorList = new();
            Dictionary<FCrew, AAthenaPlayerState[]> CrewList = new();
            List<AAthenaPlayerCharacter> PirateList = new();
            List<AAthenaPlayerCharacter> GhostPirateList = new();
            Dictionary<ABootyItemInfo, TreasureRarity> TreasureList = new();
            Dictionary<AActor, FortType> FortList = new();
            List<AMermaid> MermaidList = new();
            List<AStorm> StormList = new();
            List<FIsland> IslandList = new();
            List<UIslandDataAssetEntry> IslandDataAssetEntries = new();
            List<FCrewShipEntry> CrewShipList = new();
            Dictionary<AShip, ShipType> FarCrewShipList = new();
            Dictionary<AShip, ShipType> NearCrewShipList = new();
            List<ASharkPawn> SharkList = new();
            List<ATinyShark> MegalodonList = new();

            UWorld GWorld = new(GameManager.Memory.Read<ulong>(SigManager.UWorldAddress));

            TArray<ULocalPlayer> LocalPlayers = GWorld.OwningGameInstance.LocalPlayers;
            ulong[] LocalPlayersAddresses = LocalPlayers.GetDataPointer();
            if (LocalPlayersAddresses == null || LocalPlayersAddresses.Length == 0 || LocalPlayersAddresses[0] == 0) return;
            GameManager.LocalPlayer = new ULocalPlayer(LocalPlayersAddresses[0]);

            TArray<AActor> Actors = GWorld.PersistentLevel.AActors;

            ulong[] ActorsAddresses = Actors.GetDataPointer();

            foreach (ulong ActorAddress in ActorsAddresses)
            {
                AActor actor = new(ActorAddress);

                ActorList.Add(actor);

                if (actor.ParentObject.ClassName == "BootyItemInfo")
                {
                    ABootyItemInfo bootyInfo = new(actor.Address);

                    if (actor.ClassName.Contains("PirateLegend")) TreasureList.Add(bootyInfo, TreasureRarity.Special);
                    else if (actor.ClassName.Contains("ChestofFortune")) TreasureList.Add(bootyInfo, TreasureRarity.Special);
                    else if (actor.ClassName.Contains("piratelegend")) TreasureList.Add(bootyInfo, TreasureRarity.Legendary);
                    else if (actor.ClassName.Contains("Legendary")) TreasureList.Add(bootyInfo, TreasureRarity.Legendary);
                    else if (actor.ClassName.Contains("Gunpowder")) TreasureList.Add(bootyInfo, TreasureRarity.Danger);
                    else if (actor.ClassName.Contains("Mythical")) TreasureList.Add(bootyInfo, TreasureRarity.Mythic);
                    else if (actor.ClassName.Contains("TreasureChest")) TreasureList.Add(bootyInfo, TreasureRarity.Rare);
                    else if (actor.ClassName.Contains("TreasureArtifact")) TreasureList.Add(bootyInfo, TreasureRarity.Rare);
                    else if (actor.ClassName.Contains("Skull")) TreasureList.Add(bootyInfo, TreasureRarity.Rare);
                    //else if (actor.ClassName.Contains("Sellable")) TreasureList.Add(bootyInfo, TreasureRarity.Common);
                    //else if (actor.ClassName.Contains("Gift")) TreasureList.Add(bootyInfo, TreasureRarity.Common);
                    //else if (actor.ClassName.Contains("MermaidGem")) TreasureList.Add(bootyInfo, TreasureRarity.Common);
                    //else if (actor.ClassName.Contains("MerchantCrate")) TreasureList.Add(bootyInfo, TreasureRarity.Common);

                    else TreasureList.Add(bootyInfo, TreasureRarity.Common);
                }

                switch (actor.ClassName)
                {
                    case "BP_OnlineAthenaPlayerController_C":
                        {
                            GameManager.OnlinePlayerController = new AOnlineAthenaPlayerController(actor.Address);
                        }
                        break;

                    case "BP_PlayerPirate_C":
                        {
                            AAthenaPlayerCharacter PlayerPirate = new(actor.Address);
                            PirateList.Add(PlayerPirate);
                        }
                        break;

                    case "BP_PlayerPirate_Ghost_C":
                        {
                            AAthenaPlayerCharacter GhostPirate = new(actor.Address);
                            GhostPirateList.Add(GhostPirate);
                        }
                        break;

                    case "FOTD_Fog":
                        {
                            GameManager.FogOfTheDamned = new(actor.Address);
                        }
                        break;

                    case "BP_FogBank_C":
                        {
                            GameManager.FogBank = new(actor.Address);
                        }
                        break;

                    case "BP_SmallShipNetProxy_C":
                        {
                            AShip Ship = new(actor.Address);
                            FarCrewShipList.Add(Ship, ShipType.Sloop);
                        }
                        break;

                    case "BP_MediumShipNetProxy_C":
                        {
                            AShip Ship = new(actor.Address);
                            FarCrewShipList.Add(Ship, ShipType.Brigantine);
                        }
                        break;

                    case "BP_LargeShipNetProxy_C":
                        {
                            AShip Ship = new(actor.Address);
                            FarCrewShipList.Add(Ship, ShipType.Galleon);
                        }
                        break;

                    case "BP_SmallShipTemplate_C":
                        {
                            AShip Ship = new(actor.Address);
                            NearCrewShipList.Add(Ship, ShipType.Sloop);
                        }
                        break;

                    case "BP_MediumShipTemplate_C":
                        {
                            AShip Ship = new(actor.Address);
                            NearCrewShipList.Add(Ship, ShipType.Sloop);
                        }
                        break;

                    case "BP_LargeShipTemplate_C":
                        {
                            AShip Ship = new(actor.Address);
                            NearCrewShipList.Add(Ship, ShipType.Sloop);
                        }
                        break;

                    case "CrewService":
                        {
                            ACrewService CrewService = new(actor.Address);

                            TArray<FCrew> Crews = CrewService.Crews;

                            FCrew[] CrewAddresses = Crews.GetDataStruct();

                            foreach (FCrew Crew in CrewAddresses)
                            {
                                TArray<AAthenaPlayerState> CrewPlayers = new(Crew.Players);

                                List<AAthenaPlayerState> PlayersInCrew = new();
                                for (int i = 0; i < Crew.CrewLenght; i++)
                                {
                                    ulong CrewPlayerAddress = GameManager.Memory.Read<ulong>(Crew.Players + (ulong)i * 8);

                                    AAthenaPlayerState Player = new(CrewPlayerAddress);
                                    PlayersInCrew.Add(Player);

                                }

                                CrewList.Add(Crew, PlayersInCrew.ToArray());
                            }
                        }
                        break;

                    case "IslandService":
                        {
                            AIslandService IslandService = new(actor.Address);

                            TArray<UIslandDataAssetEntry> UIslandDataAssetEntrys = IslandService.IslandDataAsset.IslandDataEntries;
                            ulong[] UIslandDataAssetsAddresses = UIslandDataAssetEntrys.GetDataPointer();

                            foreach (ulong UIslandDataAssetAddress in UIslandDataAssetsAddresses)
                            {
                                UIslandDataAssetEntry UIslandDataAsset = new(UIslandDataAssetAddress);
                                IslandDataAssetEntries.Add(UIslandDataAsset);
                            }

                            TArray<FIsland> Islands = IslandService.IslandArray;
                            FIsland[] IslandsAddresses = Islands.GetDataStruct();

                            foreach (FIsland Island in IslandsAddresses)
                            {
                                IslandList.Add(Island);
                            }
                        }
                        break;

                    case "ShipService":
                        {
                            AShipService ShipService = new(actor.Address);

                            TArray<FCrewShipEntry> CrewedShips = ShipService.CrewedShips;
                            FCrewShipEntry[] CrewedShipsAddresses = CrewedShips.GetDataStruct();
                            foreach (FCrewShipEntry CrewedShip in CrewedShipsAddresses)
                            {
                                CrewShipList.Add(CrewedShip);
                            }
                        }
                        break;

                    case "KrakenService": // replace with actual kraken BP
                        {
                            AKrakenService KrakenService = new(actor.Address);
                            GameManager.Kraken = KrakenService.Kraken.Address == 0 ? null : KrakenService.Kraken;
                        }
                        break;

                    case "BP_Mermaid_C":
                        {
                            AMermaid Mermaid = new(actor.Address);
                            MermaidList.Add(Mermaid);
                        }
                        break;

                    case "BP_TinyShark_C":
                        {
                            ATinyShark Megalodon = new(actor.Address);
                            MegalodonList.Add(Megalodon); // recheck if thats the right class
                        }
                        break;

                    case "BP_Shark_C":
                        {
                            ASharkPawn Shark = new(actor.Address);
                            SharkList.Add(Shark); // recheck if thats the right class
                        }
                        break;

                    case "BP_Storm_C":
                        {
                            AStorm Storm = new(actor.Address);
                            StormList.Add(Storm);
                        }
                        break;

                    case "BP_SkellyShip_ShipCloud_C": 
                        {
                            FortList.Add(actor, FortType.SkeletonFleet); 
                        }
                        break;

                    case "BP_SkellyFort_SkullCloud_C":
                        {
                            FortList.Add(actor, FortType.SkeletonFort);
                        }
                        break;

                    case "BP_LegendSkellyFort_SkullCloud_C":
                        {
                            FortList.Add(actor, FortType.FortuneFort);
                        }
                        break;

                    case "BP_SkellyFort_RitualSkullCloud_C":
                        {
                            FortList.Add(actor, FortType.DamnedFort);
                        }
                        break;

                    case "BP_AshenLord_SkullCloud_C":
                        {
                            FortList.Add(actor, FortType.AshenWinds);
                        }
                        break;

                    case "BP_GhostShip_TornadoCloud_C":
                        {
                            FortList.Add(actor, FortType.GhostFleet);
                        }
                        break;
                }
            }

            GameManager.ActorList = ActorList;
            GameManager.CrewList = CrewList;
            GameManager.PirateList = PirateList;
            GameManager.GhostPirateList = GhostPirateList;
            GameManager.TreasureList = TreasureList;
            GameManager.FortList = FortList;
            GameManager.MermaidList = MermaidList;
            GameManager.StormList = StormList; 
            GameManager.IslandList = IslandList; 
            GameManager.CrewShipList = CrewShipList;
            GameManager.FarCrewShipList = FarCrewShipList;
            GameManager.NearCrewShipList = NearCrewShipList;
            GameManager.IslandDataAssetEntries = IslandDataAssetEntries;
            GameManager.SharkList = SharkList;
            GameManager.MegalodonList = MegalodonList;
        }
    }
}

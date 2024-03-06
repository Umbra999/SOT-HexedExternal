using Hexed.Memory;
using Hexed.SDK.Athena;
using Hexed.SDK.Engine;
using static Hexed.SDK.Athena.Structs;
using static Hexed.Wrappers.CustomObjects;

namespace Hexed.Core
{
    internal class GameManager
    {
        public static ProcessMemory Memory;

        public static ULocalPlayer LocalPlayer;
        public static AOnlineAthenaPlayerController OnlinePlayerController;

        public static List<AActor> ActorList = new();
        public static Dictionary<FCrew, AAthenaPlayerState[]> CrewList = new();
        public static List<AAthenaPlayerCharacter> PirateList = new();
        public static List<AAthenaPlayerCharacter> GhostPirateList = new();
        public static Dictionary<ABootyItemInfo, TreasureRarity> TreasureList = new();
        public static Dictionary<AActor, FortType> FortList = new();
        public static List<FIsland> IslandList = new();
        public static List<UIslandDataAssetEntry> IslandDataAssetEntries = new();
        public static List<FCrewShipEntry> CrewShipList = new();
        public static Dictionary<AShip, ShipType> FarCrewShipList = new();
        public static Dictionary<AShip, ShipType> NearCrewShipList = new();
        public static List<AMermaid> MermaidList = new();
        public static List<AStorm> StormList = new();
        public static List<ASharkPawn> SharkList = new();
        public static List<ATinyShark> MegalodonList = new();

        public static AKraken Kraken;
        public static AStaticMeshActor FogOfTheDamned;
        public static AFogBank FogBank;
    }
}

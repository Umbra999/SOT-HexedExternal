using Hexed.SDK.Athena;

namespace Hexed.Wrappers
{
    internal class CustomObjects
    {
        public enum TreasureRarity
        {
            Common,
            Danger,
            Rare,
            Mythic,
            Legendary,
            Special
        }

        public enum FortType
        {
            SkeletonFort,
            SkeletonFleet,
            FortuneFort,
            DamnedFort,
            GhostFleet,
            AshenWinds,
        }

        public enum ShipType
        {
            Sloop,
            Brigantine,
            Galleon,
        }

        public class DefinedCrew
        {
            public ShipType ShipType { get; set; }
            public AAthenaPlayerState[] Players { get; set; }
            public AShip CurrentShip { get; set; }
            public Guid CrewId { get; set; }
            public bool IsFarShip { get; set; }
            public bool IsOwnShip { get; set; }
        }

        public class UndefinedCrew
        {
            public ShipType ShipType { get; set; }
            public AShip ShipProxy { get; set; }
        }

        public class ToggleState
        {
            public string Name;
            public bool Toggled;
            public Action OnAction;
            public Action OffAction;
        }
    }
}

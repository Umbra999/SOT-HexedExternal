using Hexed.Core;
using Hexed.SDK.Athena;
using Hexed.SDK.Engine;
using Hexed.Wrappers;

namespace Hexed.Modules
{
    internal class CrewManager
    {
        private static AShip TryFindUniqueProxy(CustomObjects.ShipType ShipType, List<CustomObjects.DefinedCrew> definedCrews)
        {
            int ShipCount = definedCrews.Count(dc => dc.ShipType == ShipType && dc.CurrentShip == null);

            if (ShipCount == 1)
            {
                var uniqueShip = definedCrews.FirstOrDefault(dc => dc.ShipType == ShipType && dc.CurrentShip == null);
                if (uniqueShip != null)
                {
                    uniqueShip.CurrentShip = GameManager.FarCrewShipList.FirstOrDefault(kv => kv.Value == ShipType && kv.Key != uniqueShip.CurrentShip).Key;
                    return uniqueShip.CurrentShip;
                }
            }

            return null;
        }

        public static List<CustomObjects.DefinedCrew> GetDefinedCrews()
        {
            List<CustomObjects.DefinedCrew> DefinedCrews = new();

            foreach (var crew in GameManager.CrewList)
            {
                CustomObjects.DefinedCrew definedCrew = new()
                {
                    CrewId = crew.Key.CrewId
                };

                switch (crew.Key.CrewSessionTemplate.MaxMatchmakingPlayers)
                {
                    case 2:
                        definedCrew.ShipType = CustomObjects.ShipType.Sloop;
                        break;

                    case 3:
                        definedCrew.ShipType = CustomObjects.ShipType.Brigantine;
                        break;

                    case 4:
                        definedCrew.ShipType = CustomObjects.ShipType.Galleon;
                        break;
                }

                AActor CrewShip = new(GameManager.CrewShipList.Where(x => x.CrewId == crew.Key.CrewId).FirstOrDefault().Ship);

                definedCrew.CurrentShip = GameManager.NearCrewShipList.Where(x => x.Key.Address == CrewShip.Address).FirstOrDefault().Key;
                definedCrew.IsFarShip = definedCrew.CurrentShip == null;

                definedCrew.Players = crew.Value;

                var ownPlayer = definedCrew.Players.Where(x => x.PlayerId == GameManager.OnlinePlayerController.AcknowledgedPawn.PlayerState.PlayerId);
                definedCrew.IsOwnShip = ownPlayer != null && ownPlayer.ToArray().Length > 0;

                DefinedCrews.Add(definedCrew);
            }

            foreach (CustomObjects.DefinedCrew crew in DefinedCrews)
            {
                if (crew.CurrentShip != null) continue;

                crew.CurrentShip = TryFindUniqueProxy(crew.ShipType, DefinedCrews);
            }

            return DefinedCrews;
        }

        public static List<CustomObjects.UndefinedCrew> GetUndefinedCrews()
        {
            List<CustomObjects.DefinedCrew> definedCrews = GetDefinedCrews();
            List<CustomObjects.UndefinedCrew> unknownProxies = new();

            foreach (var shipProxy in GameManager.FarCrewShipList)
            {
                bool IsNear = false;

                foreach (var definedCrew in definedCrews)
                {
                    if (definedCrew.CurrentShip == null) continue;

                    int DistanceToPlayer = GameHelper.GetDistanceInMeter(shipProxy.Key.RootComponent.Transform.Translation);
                    int DistanceToDefinedShip = GameHelper.GetDistanceInMeter(shipProxy.Key.RootComponent.Transform.Translation, definedCrew.CurrentShip.RootComponent.Transform.Translation);

                    if (DistanceToPlayer < 1700 || DistanceToDefinedShip < 40) IsNear = true; // replace with reliable check one day
                }

                if (!IsNear)
                {
                    CustomObjects.UndefinedCrew undefinedCrew = new()
                    {
                        ShipProxy = shipProxy.Key,
                        ShipType = shipProxy.Value
                    };

                    unknownProxies.Add(undefinedCrew);
                }
            }

            return unknownProxies;
        }
    }
}

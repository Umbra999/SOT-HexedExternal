using Hexed.SDK.Offsets;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using static Hexed.SDK.Engine.Structs;
using static Hexed.SDK.Offsets.EnumOffsets;

namespace Hexed.SDK.Athena
{
    internal class Structs
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct FIsland
        {
            [FieldOffset(StructOffsets.FIsland.IslandName)]
            public ulong IslandName; // FName

            [FieldOffset(StructOffsets.FIsland.IslandType)]
            public EIslandType IslandType;

            [FieldOffset(StructOffsets.FIsland.Sea)]
            public ulong Sea; // USeaId

            [FieldOffset(StructOffsets.FIsland.IslandBoundsCentre)]
            public Vector3 IslandBoundsCentre;

            [FieldOffset(StructOffsets.FIsland.IslandBoundsRadius)]
            public float IslandBoundsRadius;

            [FieldOffset(StructOffsets.FIsland.IslandTriggerRadius)]
            public float IslandTriggerRadius;

            [FieldOffset(StructOffsets.FIsland.IslandSafeZoneRadius)]
            public float IslandSafeZoneRadius;

            [FieldOffset(StructOffsets.FIsland.ShipDiveAndResurfaceExclusionZoneRadius)]
            public float ShipDiveAndResurfaceExclusionZoneRadius;

            [FieldOffset(StructOffsets.FIsland.Rotation)]
            public float Rotation;

            [FieldOffset(StructOffsets.FIsland.CompassDirectionIslandCentre)]
            public Vector3 CompassDirectionIslandCentre;

            [FieldOffset(StructOffsets.FIsland.PetMovementParamsData)]
            public ulong PetMovementParamsData;

            [FieldOffset(StructOffsets.FIsland.IslandFeatureNames)]
            public ulong IslandFeatureNames; // FName TArray
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FCrew
        {
            [FieldOffset(StructOffsets.FCrew.CrewId)]
            public Guid CrewId;

            [FieldOffset(StructOffsets.FCrew.SessionId)]
            public Guid SessionId;

            [FieldOffset(StructOffsets.FCrew.Players)]
            public ulong Players; // TArray<APlayerState*>

            [FieldOffset(StructOffsets.FCrew.Players + 8)] // Undocumented but + 8 cuz array
            public int CrewLenght;

            [FieldOffset(StructOffsets.FCrew.CrewSessionTemplate)]
            public FCrewSessionTemplate CrewSessionTemplate;

            [FieldOffset(StructOffsets.FCrew.LiveryID)]
            public Guid LiveryID;

            [FieldOffset(StructOffsets.FCrew.AssociatedActors)]
            public ulong AssociatedActors; // TArray<AActor*>

            [FieldOffset(StructOffsets.FCrew.HasEverSetSail)]
            public bool HasEverSetSail;

            [FieldOffset(StructOffsets.FCrew.ScrambleNameIndex)]
            public int ScrambleNameIndex;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FCrewSessionTemplate
        {
            [FieldOffset(StructOffsets.FCrewSessionTemplate.MatchmakingHopper)]
            public FString MatchmakingHopper;

            [FieldOffset(StructOffsets.FCrewSessionTemplate.ShipSize)]
            public ulong ShipSize;

            [FieldOffset(StructOffsets.FCrewSessionTemplate.MaxMatchmakingPlayers)]
            public int MaxMatchmakingPlayers;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FCrewShipEntry
        {
            [FieldOffset(StructOffsets.FCrewShipEntry.CrewId)]
            public Guid CrewId;

            [FieldOffset(StructOffsets.FCrewShipEntry.Ship)]
            public ulong Ship; // AActor*
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FWeakActorHandle
        {
            [FieldOffset(StructOffsets.FWeakActorHandle.ActorId)]
            public FActorId ActorId;

            [FieldOffset(StructOffsets.FWeakActorHandle.NetActorPtr)]
            public FNetActorPtr NetActorPtr;

            [FieldOffset(StructOffsets.FWeakActorHandle.Valid)]
            public bool Valid;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FHealthRegenState
        {
            [FieldOffset(StructOffsets.FHealthRegenState.CurrentPoolAmount)]
            public float CurrentPoolAmount;

            [FieldOffset(StructOffsets.FHealthRegenState.CurrentPoolAmount)]
            public ERegenerationState State;
        }
    }
}

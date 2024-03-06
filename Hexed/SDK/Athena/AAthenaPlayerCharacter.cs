using Hexed.Core;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Offsets.EnumOffsets;

namespace Hexed.SDK.Athena
{
    internal class AAthenaPlayerCharacter : AAthenaCharacter
    {
        public AAthenaPlayerCharacter(ulong address) : base(address) { }

        public UDrowningComponent DrowningComponent
        {
            get
            {
                return new UDrowningComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaPlayerCharacter.DrowningComponent));
            }
        }

        public UDrunkennessComponent DrunkennessComponent
        {
            get
            {
                return new UDrunkennessComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaPlayerCharacter.DrunkennessComponent));
            }
        }

        public USicknessComponent SicknessComponent
        {
            get
            {
                return new USicknessComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaPlayerCharacter.SicknessComponent));
            }
        }

        public UBurpComponent BurpComponent
        {
            get
            {
                return new UBurpComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaPlayerCharacter.BurpComponent));
            }
        }

        public ULimpingComponent LimpingComponent
        {
            get
            {
                return new ULimpingComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaPlayerCharacter.LimpingComponent));
            }
        }

        public UClimbingComponent ClimbingComponent
        {
            get
            {
                return new UClimbingComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AAthenaPlayerCharacter.ClimbingComponent));
            }
        }

        public bool PreventJumping
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AAthenaPlayerCharacter.PreventJumping);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AAthenaPlayerCharacter.PreventJumping, value);
            }
        }

        public bool StopMovementAndPreventSwimming
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AAthenaPlayerCharacter.StopMovementAndPreventSwimming);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AAthenaPlayerCharacter.StopMovementAndPreventSwimming, value);
            }
        }

        public bool FinishedWaitingForSpawn
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AAthenaPlayerCharacter.FinishedWaitingForSpawn);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AAthenaPlayerCharacter.FinishedWaitingForSpawn, value);
            }
        }

        public bool ReplicatedEmoteExitAllowed
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AAthenaPlayerCharacter.ReplicatedEmoteExitAllowed);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AAthenaPlayerCharacter.ReplicatedEmoteExitAllowed, value);
            }
        }

        public ECharacterType CharacterType
        {
            get
            {
                return GameManager.Memory.Read<ECharacterType>(Address + ClassOffsets.AAthenaPlayerCharacter.CharacterType);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AAthenaPlayerCharacter.CharacterType, value);
            }
        }
    }
}

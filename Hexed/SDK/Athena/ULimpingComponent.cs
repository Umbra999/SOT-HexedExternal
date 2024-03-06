using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class ULimpingComponent : UActorComponent
    {
        public ULimpingComponent(ulong address) : base(address) { }

        public int CheatPunishmentIncreasePerViolation
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + ClassOffsets.ULimpingComponent.CheatPunishmentIncreasePerViolation);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.ULimpingComponent.CheatPunishmentIncreasePerViolation, value);
            }
        }

        public int NumAntiCheatSamplesOnServer
        {
            get
            {
                return GameManager.Memory.Read<int>(Address + ClassOffsets.ULimpingComponent.NumAntiCheatSamplesOnServer);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.ULimpingComponent.NumAntiCheatSamplesOnServer, value);
            }
        }

        public float DelayUntilStartCheatDetection
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.ULimpingComponent.DelayUntilStartCheatDetection);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.ULimpingComponent.DelayUntilStartCheatDetection, value);
            }
        }
    }
}

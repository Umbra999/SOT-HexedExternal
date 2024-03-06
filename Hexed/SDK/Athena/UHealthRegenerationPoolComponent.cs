using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Athena.Structs;

namespace Hexed.SDK.Athena
{
    internal class UHealthRegenerationPoolComponent : UActorComponent
    {
        public UHealthRegenerationPoolComponent(ulong address) : base(address) { }

        public float MaxCapacity
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UHealthRegenerationPoolComponent.MaxCapacity);
            }
        }

        public float HealingRate
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UHealthRegenerationPoolComponent.HealingRate);
            }
        }

        public float HealingDelayWhenDamaged
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UHealthRegenerationPoolComponent.HealingDelayWhenDamaged);
            }
        }

        public FHealthRegenState RegenerationState
        {
            get
            {
                return GameManager.Memory.Read<FHealthRegenState>(Address + ClassOffsets.UHealthRegenerationPoolComponent.RegenerationState);
            }
        }
    }
}

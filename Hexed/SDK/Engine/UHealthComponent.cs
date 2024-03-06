using Hexed.Core;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Engine.Structs;

namespace Hexed.SDK.Engine
{
    internal class UHealthComponent : UActorComponent
    {
        public UHealthComponent(ulong address) : base(address) { }

        public FCurrentHealthInfo CurrentHealthInfo
        {
            get
            {
                return GameManager.Memory.Read<FCurrentHealthInfo>(Address + ClassOffsets.UHealthComponent.CurrentHealthInfo);
            }
        }

        public float MaxHealth
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UHealthComponent.MaxHealth);
            }
        }
    }
}

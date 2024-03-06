using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class UBurpComponent : UActorComponent
    {
        public UBurpComponent(ulong address) : base(address) { }

        public float MinGasToTriggerBurp
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UBurpComponent.MinGasToTriggerBurp);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UBurpComponent.MinGasToTriggerBurp, value);
            }
        }
    }
}

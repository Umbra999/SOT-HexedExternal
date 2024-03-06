using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class UDrunkennessComponent : UActorComponent
    {
        public UDrunkennessComponent(ulong address) : base(address) { }

        public float CurrentDrunkenness0x02
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UDrunkennessComponent.CurrentDrunkenness0x02);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UDrunkennessComponent.CurrentDrunkenness0x02, value);
            }
        }
    }
}

using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class UClimbingComponent : UIntentComponent
    {
        public UClimbingComponent(ulong address) : base(address) { }

        public float ServerHeight
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UClimbingComponent.ServerHeight);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UClimbingComponent.ServerHeight, value);
            }
        }
    }
}

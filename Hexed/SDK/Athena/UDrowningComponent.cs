using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class UDrowningComponent : UActorComponent
    {
        public UDrowningComponent(ulong address) : base(address) { }

        public float OxygenLevel
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.UDrowningComponent.OxygenLevel);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UDrowningComponent.OxygenLevel, value);
            }
        }

        public bool IsDrowningDisabled
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.UDrowningComponent.IsDrowningDisabled);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.UDrowningComponent.IsDrowningDisabled, value);
            }
        }
    }
}

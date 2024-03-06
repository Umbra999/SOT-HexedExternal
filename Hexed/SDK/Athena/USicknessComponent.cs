using Hexed.Core;
using Hexed.SDK.Engine;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Athena
{
    internal class USicknessComponent : UActorComponent
    {
        public USicknessComponent(ulong address) : base(address) { }

        public bool CanGetSick
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.USicknessComponent.CanGetSick);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.USicknessComponent.CanGetSick, value);
            }
        }

        public float TargetScreenEffectStrength
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.USicknessComponent.TargetScreenEffectStrength);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.USicknessComponent.TargetScreenEffectStrength, value);
            }
        }

        public float ScreenEffectStrengthWhenSicknessActive
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.USicknessComponent.ScreenEffectStrengthWhenSicknessActive);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.USicknessComponent.ScreenEffectStrengthWhenSicknessActive, value);
            }
        }

        public float ScreenEffectStrengthWhileVomiting
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.USicknessComponent.ScreenEffectStrengthWhileVomiting);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.USicknessComponent.ScreenEffectStrengthWhileVomiting, value);
            }
        }
    }
}

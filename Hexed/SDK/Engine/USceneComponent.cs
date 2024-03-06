using Hexed.Core;
using static Hexed.SDK.Engine.Structs;

namespace Hexed.SDK.Engine
{
    internal class USceneComponent : UActorComponent
    {
        public USceneComponent(ulong address) : base(address) { }

        public FTransform Transform // Undocumented but always 0x150
        {
            get
            {
                return GameManager.Memory.Read<FTransform>(Address + 0x150);
            }
            set
            {
                GameManager.Memory.Write(Address + 0x150, value);
            }
        }
    }
}

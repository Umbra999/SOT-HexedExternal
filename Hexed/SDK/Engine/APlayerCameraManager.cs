using Hexed.Core;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Engine.Structs;

namespace Hexed.SDK.Engine
{
    internal class APlayerCameraManager : AActor
    {
        public APlayerCameraManager(ulong address) : base(address) { }

        public USceneComponent TransformComponent
        {
            get
            {
                return new USceneComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.APlayerCameraManager.TransformComponent));
            }
        }

        public float DefaultFOV
        {
            get
            {
                return GameManager.Memory.Read<float>(Address + ClassOffsets.APlayerCameraManager.DefaultFOV);
            }
        }

        public FTViewTarget ViewTarget
        {
            get
            {
                return GameManager.Memory.Read<FTViewTarget>(Address + ClassOffsets.APlayerCameraManager.ViewTarget);
            }
        }
    }
}

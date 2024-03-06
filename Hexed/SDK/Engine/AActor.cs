using Hexed.Core;
using Hexed.SDK.Offsets;
using static Hexed.SDK.Engine.Structs;

namespace Hexed.SDK.Engine
{
    internal class AActor : UObject
    {
        public AActor(ulong address) : base(address) { }

        public FRepMovement ReplicatedMovement
        {
            get
            {
                return GameManager.Memory.Read<FRepMovement>(Address + ClassOffsets.AActor.ReplicatedMovement);
            }
        }

        public USceneComponent RootComponent
        {
            get
            {
                return new USceneComponent(GameManager.Memory.Read<ulong>(Address + ClassOffsets.AActor.RootComponent));
            }
        }

        public ulong HiddenEditorViews
        {
            get
            {
                return GameManager.Memory.Read<ulong>(Address + ClassOffsets.AActor.HiddenEditorViews);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AActor.HiddenEditorViews, value);
            }
        }

        public FActorPtr ParentComponentActor
        {
            get
            {
                return GameManager.Memory.Read<FActorPtr>(Address + ClassOffsets.AActor.ParentComponentActor);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AActor.ParentComponentActor, value);
            }
        }
    }
}

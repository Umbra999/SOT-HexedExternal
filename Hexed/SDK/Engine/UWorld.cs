using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class UWorld : UObject
    {
        public UWorld(ulong address) : base(address) { }

        public ULevel PersistentLevel
        {
            get
            {
                return new ULevel(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UWorld.PersistentLevel));
            }
        }

        public ULevel CurrentLevel
        {
            get
            {
                return new ULevel(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UWorld.CurrentLevel));
            }
        }

        public TArray<ULevel> Levels
        {
            get
            {
                return new TArray<ULevel>(Address + ClassOffsets.UWorld.Levels);
            }
        }


        public AGameState GameState
        {
            get
            {
                return new AGameState(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UWorld.GameState));
            }
        }

        public UGameInstance OwningGameInstance
        {
            get
            {
                return new UGameInstance(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UWorld.OwningGameInstance));
            }
        }
    }
}

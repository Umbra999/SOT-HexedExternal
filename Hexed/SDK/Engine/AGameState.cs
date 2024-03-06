using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class AGameState : AInfo
    {
        public AGameState(ulong address) : base(address) { }

        public TArray<APlayerState> PlayerArray
        {
            get
            {
                return new TArray<APlayerState>(Address + ClassOffsets.AGameState.PlayerArray);
            }
        }
    }
}

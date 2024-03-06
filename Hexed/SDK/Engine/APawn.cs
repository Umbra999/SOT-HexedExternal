using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class APawn : AActor
    {
        public APawn(ulong address) : base(address) { }

        public APlayerState PlayerState
        {
            get
            {
                return new APlayerState(GameManager.Memory.Read<ulong>(Address + ClassOffsets.APawn.PlayerState));
            }
        }
    }
}

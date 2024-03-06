using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class UGameViewportClient : UScriptViewportClient 
    {
        public UGameViewportClient(ulong address) : base(address) { }

        public UWorld World
        {
            get
            {
                return new UWorld(GameManager.Memory.Read<ulong>(Address + ClassOffsets.UGameViewportClient.World));
            }
        }
    }
}

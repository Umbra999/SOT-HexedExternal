using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class UGameInstance : UObject
    {
        public UGameInstance(ulong address) : base(address) { }

        public TArray<ULocalPlayer> LocalPlayers
        {
            get
            {
                return new TArray<ULocalPlayer>(Address + ClassOffsets.UGameInstance.LocalPlayers);
            }
        }
    }
}

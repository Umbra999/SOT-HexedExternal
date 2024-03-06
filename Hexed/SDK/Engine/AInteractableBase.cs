using Hexed.Core;
using Hexed.SDK.Offsets;

namespace Hexed.SDK.Engine
{
    internal class AInteractableBase : AActor
    {
        public AInteractableBase(ulong address) : base(address) { }

        public bool RequiresFacingFront
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AInteractableBase.RequiresFacingFront);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AInteractableBase.RequiresFacingFront, value);
            }
        }

        public bool RequiresNotBeingAirborne
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AInteractableBase.RequiresNotBeingAirborne);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AInteractableBase.RequiresNotBeingAirborne, value);
            }
        }

        public bool RequiresNotSwimming
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AInteractableBase.RequiresNotSwimming);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AInteractableBase.RequiresNotSwimming, value);
            }
        }

        public bool InteractionsCanBeDisabled
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AInteractableBase.InteractionsCanBeDisabled);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AInteractableBase.InteractionsCanBeDisabled, value);
            }
        }

        public bool CanSetInteractionState
        {
            get
            {
                return GameManager.Memory.Read<bool>(Address + ClassOffsets.AInteractableBase.CanSetInteractionState);
            }
            set
            {
                GameManager.Memory.Write(Address + ClassOffsets.AInteractableBase.CanSetInteractionState, value);
            }
        }
    }
}

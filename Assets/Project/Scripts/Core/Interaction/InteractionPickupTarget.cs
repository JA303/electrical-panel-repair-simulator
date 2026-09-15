using Project.Scripts.Core.Item;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Core.Interaction
{
    public class InteractionPickupTarget : InteractionTarget
    {
        [SerializeField] private PickableItem item;

        public override bool CanInteract(
            MissionRuntime mission,
            PlayerHands hands)
        {
            return !item.HasPickup && hands.IsEmpty;
        }

        public override bool TryInteract(
            MissionRuntime mission,
            PlayerInteractor playerInteractor)
        {
            if (item.HasPickup && playerInteractor.Hands.IsEmpty)
                return false;
            
            playerInteractor.Hands.TryPick(item);
            return true;
        }
    }
}
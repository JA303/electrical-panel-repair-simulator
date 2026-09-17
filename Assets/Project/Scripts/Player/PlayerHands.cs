using Project.Scripts.Core.Item;
using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerHands : MonoBehaviour
    {
        [SerializeField] private PlayerControlGate controlGate;
        [SerializeField] private Transform holdPoint;

        private PickableItem heldItem;

        public bool IsEmpty => heldItem == null;
        public PickableItem HeldItem => heldItem;
        public ItemDefinition HeldItemDefinition =>
            heldItem != null ? heldItem.ItemDefinition : null;

        private void Update()
        {
            SetVisibility(!controlGate.IsLocked);
        }

        public bool HasItem(ItemDefinition item)
        {
            return heldItem != null &&
                   heldItem.ItemDefinition == item;
        }

        public bool TryPick(PickableItem item)
        {
            if (item == null || !IsEmpty)
                return false;

            heldItem = item;
            item.AttachToHand(holdPoint);
            return true;
        }

        public PickableItem Drop()
        {
            if (heldItem == null)
                return null;

            PickableItem item = heldItem;
            heldItem = null;
            item.DetachFromHand();
            return item;
        }

        public PickableItem ConsumeHeldItem(ItemDefinition expectedItem)
        {
            if (!HasItem(expectedItem))
                return null;

            PickableItem item = heldItem;
            heldItem = null;
            item.Consume();
            return item;
        }

        private void SetVisibility(bool visible)
        {
            holdPoint.gameObject.SetActive(visible);
        }
    }
}
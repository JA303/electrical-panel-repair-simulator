using UnityEngine;

namespace Project.Scripts.Core.Item
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private ItemDefinition itemDefinition;
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider[] colliders;

        private Transform originalParent;

        public ItemDefinition ItemDefinition => itemDefinition;
        public bool HasPickup { get; private set; }

        private void Awake()
        {
            originalParent = transform.parent;
        }

        public void AttachToHand(Transform holdPoint)
        {
            if (holdPoint == null)
                return;

            HasPickup = true;

            body.isKinematic = true;
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;

            foreach (Collider collider in colliders)
                collider.enabled = false;

            transform.SetParent(holdPoint);
            transform.localPosition = itemDefinition.HoldLocalPosition;
            transform.localEulerAngles = itemDefinition.HoldLocalEulerAngles;
        }

        public void DetachFromHand()
        {
            HasPickup = false;
            
            transform.SetParent(originalParent);

            foreach (Collider collider in colliders)
                collider.enabled = true;

            body.isKinematic = false;
            body.AddForce(transform.forward * 10);
        }

        public void Consume()
        {
            Destroy(gameObject);
        }
    }
}
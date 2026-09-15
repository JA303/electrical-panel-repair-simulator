using Project.Scripts.Core.Interaction;
using Project.Scripts.Core.Mission;
using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactionMask;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerHands hands;
        [SerializeField] private PlayerControlGate controlGate;
        [SerializeField] private MissionRuntime mission;

        private InteractionTarget currentTarget;

        public PlayerHands Hands => hands;
        public PlayerControlGate ControlGate => controlGate;

        private void Update()
        {
            if (controlGate && controlGate.IsLocked)
            {
                ClearCurrentTarget();
                return;
            }

            RefreshTarget();

            if (playerInput.InteractInput.WasPressedThisFrame())
                TryInteract();
            
            if (playerInput.DropInput.WasPressedThisFrame())
                TryDrop();
        }

        private void RefreshTarget()
        {
            Ray ray = new Ray(
                playerCamera.transform.position,
                playerCamera.transform.forward);

#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance);
#endif

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    interactionDistance,
                    interactionMask))
            {
                ClearCurrentTarget();
                return;
            }

            InteractionTarget target =
                hit.collider.GetComponentInParent<InteractionTarget>();

            if (!target || !target.CanInteract(mission, hands))
            {
                ClearCurrentTarget();
                return;
            }

            ClearCurrentTarget();
            currentTarget = target;
            currentTarget.SetHighlight(true);
        }

        private void TryInteract()
        {
            currentTarget?.TryInteract(mission, this);
        }

        private void TryDrop()
        {
            if (hands && !hands.IsEmpty)
                hands.Drop();
        }

        private void ClearCurrentTarget()
        {
            currentTarget?.SetHighlight(false);
            currentTarget = null;
        }
    }
}
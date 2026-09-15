using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Core.Interaction
{
    public class InteractionTarget : MonoBehaviour
    {
        [SerializeField] private InteractionAction[] actions;
        [SerializeField] private Renderer[] highlightRenderers;

        public InteractionAction[] Actions => actions;

        public InteractionAction FindAvailableAction(
            MissionRuntime mission,
            PlayerHands hands)
        {
            if (mission == null || hands == null)
                return null;

            foreach (InteractionAction action in actions)
            {
                if (action == null || action.task == null)
                    continue;

                if (!mission.CanStart(action.task))
                    continue;

                if (!HasRequiredHand(action, hands))
                    continue;

                return action;
            }

            return null;
        }

        private bool HasRequiredHand(
            InteractionAction action,
            PlayerHands hands)
        {
            switch (action.task.RequiredHand)
            {
                case RequiredHand.Any:
                    return true;

                case RequiredHand.Empty:
                    return hands.IsEmpty;

                case RequiredHand.SpecificItem:
                    return hands.HasItem(action.requiredItem);

                default:
                    return false;
            }
        }

        public void SetHighlight(bool active)
        {
            foreach (Renderer renderer in highlightRenderers)
            {
                if (renderer != null)
                    renderer.material.SetFloat("_OutlineWidth", active ? 1f : 0f);
            }
        }
    }
}
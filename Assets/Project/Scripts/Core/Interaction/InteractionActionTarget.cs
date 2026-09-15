using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Core.Interaction
{
    public class InteractionActionTarget : InteractionTarget
    {
        [SerializeField] private InteractionAction[] actions;

        public override bool CanInteract(
            MissionRuntime mission,
            PlayerHands hands)
        {
            return FindAvailableAction(mission, hands) is not null;
        }

        public override bool TryInteract(
            MissionRuntime mission,
            PlayerInteractor playerInteractor)
        {
            InteractionAction action = FindAvailableAction(mission, playerInteractor.Hands);

            if (action == null)
                return false;

            action.executor.Execute(action.task, playerInteractor);
            return true;
        }

        private InteractionAction FindAvailableAction(
            MissionRuntime mission,
            PlayerHands hands)
        {
            if (!mission || !hands)
                return null;

            foreach (InteractionAction action in actions)
            {
                if (action == null || !action.task)
                    continue;

                if (!mission.CanStart(action.task))
                    continue;

                if (!HasRequiredHand(action, hands))
                    continue;

                return action;
            }

            return null;
        }

        private static bool HasRequiredHand(
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
    }
}
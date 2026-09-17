using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay.WireSlots
{
    public class UnscrewExecutor : TaskActionExecutor
    {
        [SerializeField] private WireSlot wireSlot;
        [SerializeField] private bool leftScrew;

        [SerializeField] private Animator animator;
        [SerializeField] private string unscrewTrigger = "Unscrew";

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed)
        {
            if (!interactor.Hands.HasItem(task.RequiredItem))
            {
                completed(false);
                yield break;
            }

            animator.SetTrigger(unscrewTrigger);

            yield return new WaitForSeconds(1f);

            if (leftScrew)
                wireSlot.MarkLeftScrewOpen();
            else
                wireSlot.MarkRightScrewOpen();

            completed(true);
        }
    }
}
using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay
{
    public class ClosePanelDoorExecutor : TaskActionExecutor
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string closeTrigger = "Close";

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed)
        {
            if (!interactor.Hands.IsEmpty)
            {
                completed(false);
                yield break;
            }

            animator.SetTrigger(closeTrigger);

            yield return new WaitForSeconds(1.5f);

            completed(true);
        }
    }
}
using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay
{
    public class OpenPanelDoorExecutor : TaskActionExecutor
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string openTrigger = "Open";

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

            animator.SetTrigger(openTrigger);

            yield return new WaitForSeconds(1.5f);

            completed(true);
        }
    }
}
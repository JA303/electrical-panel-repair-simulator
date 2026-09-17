using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay
{
    public class UnlockPanelExecutor : TaskActionExecutor
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string unlockTrigger = "Unlock";
        

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
            
            animator.SetTrigger(unlockTrigger);

            yield return new WaitForSeconds(1.2f);
            
            completed(true);
        }
    }
}
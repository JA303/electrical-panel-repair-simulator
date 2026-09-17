using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay
{
    public class SwitchOnMainFuseExecutor : TaskActionExecutor
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string switchOffTrigger = "SwitchOn";
        
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

            animator.SetTrigger(switchOffTrigger);

            yield return new WaitForSeconds(1f);

            completed(true);
        }
    }
}
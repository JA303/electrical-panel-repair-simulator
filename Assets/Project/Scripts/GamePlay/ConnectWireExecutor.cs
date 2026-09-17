using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay
{
    public class ConnectWireExecutor : TaskActionExecutor
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string connectTrigger = "Connect";

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

            animator.SetTrigger(connectTrigger);

            yield return new WaitForSeconds(1.8f);

            completed(true);
        }
    }
}
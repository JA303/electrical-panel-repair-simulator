using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay.WireSlots
{
    public class TightenScrewExecutor : TaskActionExecutor
    {
        [SerializeField] private WireSlot wireSlot;
        [SerializeField] private bool leftScrew;

        [SerializeField] private Animator animator;
        [SerializeField] private string tightenTrigger = "Tighten";

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed)
        {
            if (!interactor.Hands.HasItem(task.RequiredItem) || !wireSlot.IsReadyForTightening)
            {
                completed(false);
                yield break;
            }

            animator.SetTrigger(tightenTrigger);

            yield return new WaitForSeconds(1f);

            if (leftScrew)
                wireSlot.TightenLeftScrew();
            else
                wireSlot.TightenRightScrew();

            completed(true);
        }
    }
}
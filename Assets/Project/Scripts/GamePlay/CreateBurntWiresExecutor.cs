using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.GamePlay.WireSlots;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay
{
    public class CreateBurntWiresExecutor : TaskActionExecutor
    {
        [SerializeField] private WireSlot[] wireSlots;
        
        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed)
        {
            if (wireSlots == null || wireSlots.Length == 0)
            {
                completed(false);
                yield break;
            }

            foreach (WireSlot slot in wireSlots)
            {
                if (!slot)
                    continue;

                slot.SetBurnt();
            }

            completed(true);
        }
    }
}
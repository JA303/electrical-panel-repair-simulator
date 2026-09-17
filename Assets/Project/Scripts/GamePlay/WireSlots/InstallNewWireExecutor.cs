using System.Collections;
using Project.Scripts.Core.Item;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay.WireSlots
{
    public class InstallNewWireExecutor : TaskActionExecutor
    {
        [SerializeField] private WireSlot wireSlot;
        [SerializeField] private ItemDefinition newWireItem;
        
        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed)
        {
            if (!interactor.Hands.HasItem(newWireItem) || !wireSlot.IsRemoved)
            {
                completed(false);
                yield break;
            }

            wireSlot.InstallNewWire();
            interactor.Hands.ConsumeHeldItem(newWireItem);

            completed(true);
        }
    }
}
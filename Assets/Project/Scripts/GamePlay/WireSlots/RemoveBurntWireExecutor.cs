using System.Collections;
using Project.Scripts.Core.Item;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay.WireSlots
{
    public class RemoveBurntWireExecutor : TaskActionExecutor
    {
        [SerializeField] private WireSlot wireSlot;
        [SerializeField] private ItemDefinition burntWireItem;
        
        [SerializeField] private PickableItem burntWirePickupPrefab;
        [SerializeField] private Transform dropPoint;

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed)
        {
            if (!interactor.Hands.IsEmpty || !wireSlot.IsReadyForRemoval)
            {
                completed(false);
                yield break;
            }

            wireSlot.RemoveBurntWire();

            if (burntWirePickupPrefab && dropPoint)
            {
                PickableItem item =
                    Instantiate(
                        burntWirePickupPrefab,
                        dropPoint.position,
                        dropPoint.rotation);

                interactor.Hands.TryPick(item);
            }

            completed(true);
        }
    }
}
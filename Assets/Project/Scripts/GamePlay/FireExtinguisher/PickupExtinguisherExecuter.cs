using System;
using System.Collections;
using Project.Scripts.Core.Item;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay.FireExtinguisher
{
    public class PickupExtinguisherExecuter : TaskActionExecutor
    {
        [SerializeField] private ItemDefinition executerItem;

        [SerializeField] private GameObject executerVisual;
        [SerializeField] private PickableItem executerPickupPrefab;
        [SerializeField] private Transform dropPoint;

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            Action<bool> completed)
        {
            if (!interactor.Hands.IsEmpty)
            {
                completed(false);
                yield break;
            }
            
            if (executerPickupPrefab && dropPoint)
            {
                PickableItem item =
                    Instantiate(
                        executerPickupPrefab,
                        dropPoint.position,
                        dropPoint.rotation);

                interactor.Hands.TryPick(item);
            }

            completed(true);
            
            executerVisual.SetActive(false);
        }
    }
}
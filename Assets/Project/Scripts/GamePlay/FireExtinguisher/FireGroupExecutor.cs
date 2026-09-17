using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.GamePlay.FireExtinguisher
{
    public class FireGroupExecutor : TaskActionExecutor
    {
        public static FireGroupExecutor Instance { get; private set; }
        
        [SerializeField] private FireController[] fires;

        private readonly HashSet<FireController> extinguish = new();
        private bool allFiresExtinguished;

        private void Awake()
        {
            if (Instance is null)
                Instance = this;
            else
                Destroy(this);
            
            foreach (var fire in fires)
                extinguish.Add(fire);

            allFiresExtinguished = false;
        }
        

        public void NotifyFireExtinguished(FireController fire)
        {
            if (!fire)
                return;

            extinguish.Remove(fire);
            allFiresExtinguished = extinguish.Count == 0;
        }

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            Action<bool> completed)
        {
            if (fires is null || fires.Length == 0)
                completed(false);

            EnableAllFires();
            
            yield return new WaitUntil(() => allFiresExtinguished);

            completed(true);
        }

        private void EnableAllFires()
        {
            foreach (var fire in fires)
                fire.gameObject.SetActive(true);
        }
    }
}
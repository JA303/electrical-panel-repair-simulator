using System;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Core.Interaction
{
    public abstract class InteractionTarget : MonoBehaviour
    {
        [SerializeField] private Outline outline;
        
        private void Start()
        {
            outline.enabled = false;
        }

        public abstract bool CanInteract(
            MissionRuntime mission,
            PlayerHands hands);

        public abstract bool TryInteract(
            MissionRuntime mission,
            PlayerInteractor playerInteractor);

        public void SetHighlight(bool active)
        {
            outline.enabled = active;
        }
    }
}
using UnityEngine;

namespace Project.Scripts.GamePlay.WireSlots
{
    public class WireState : MonoBehaviour
    {
        [SerializeField] private GameObject healthyVisual;
        [SerializeField] private GameObject burntVisual;
        [SerializeField] private GameObject newWireVisual;

        public WireVisualState CurrentState { get; private set; }

        public bool IsHealthy =>
            CurrentState == WireVisualState.Healthy;

        public bool IsBurnt =>
            CurrentState == WireVisualState.Burnt;

        public bool IsRemoved =>
            CurrentState == WireVisualState.Removed;

        public bool IsNewInstalled =>
            CurrentState == WireVisualState.NewInstalled;

        private void Start()
        {
            // SetState(WireVisualState.Healthy);
        }

        public void SetBurnt()
        {
            if (CurrentState != WireVisualState.Healthy)
                return;

            SetState(WireVisualState.Burnt);
        }

        public void RemoveBurntWire()
        {
            if (!IsBurnt)
                return;

            SetState(WireVisualState.Removed);
        }

        public void InstallNewWire()
        {
            if (!IsRemoved)
                return;

            SetState(WireVisualState.NewInstalled);
        }

        private void SetState(WireVisualState newState)
        {
            CurrentState = newState;

            if (healthyVisual)
                healthyVisual.SetActive(newState == WireVisualState.Healthy);

            if (burntVisual)
                burntVisual.SetActive(newState == WireVisualState.Burnt);

            if (newWireVisual)
                newWireVisual.SetActive(newState == WireVisualState.NewInstalled);
        }
    }
}
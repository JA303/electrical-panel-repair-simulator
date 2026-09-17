using UnityEngine;

namespace Project.Scripts.GamePlay.WireSlots
{
    public class WireSlot : MonoBehaviour
    {
        [SerializeField] private WireState wireState;

        private bool leftScrewOpen;
        private bool rightScrewOpen;
        
        public bool IsBurnt => wireState && wireState.IsBurnt;
        public bool IsRemoved => wireState && wireState.IsRemoved;
        public bool IsNewInstalled => wireState && wireState.IsNewInstalled;

        public bool IsReadyForRemoval =>
            IsBurnt &&
            leftScrewOpen &&
            rightScrewOpen;

        public bool IsReadyForTightening => IsNewInstalled;

        public void SetBurnt()
        {
            wireState.SetBurnt();

            leftScrewOpen = false;
            rightScrewOpen = false;
        }

        public void MarkLeftScrewOpen()
        {
            if (!IsBurnt)
                return;

            leftScrewOpen = true;
        }

        public void MarkRightScrewOpen()
        {
            if (!IsBurnt)
                return;

            rightScrewOpen = true;
        }

        public void RemoveBurntWire()
        {
            if (!IsReadyForRemoval)
                return;

            wireState.RemoveBurntWire();
        }

        public void InstallNewWire()
        {
            wireState.InstallNewWire();
        }

        public void TightenLeftScrew()
        {
            if (!IsNewInstalled)
                return;

            leftScrewOpen = false;
        }

        public void TightenRightScrew()
        {
            if (!IsNewInstalled)
                return;

            rightScrewOpen = false;
        }
    }
}
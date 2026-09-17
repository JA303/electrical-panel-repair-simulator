using Project.Scripts.Core.Mission;
using UnityEngine;

namespace Project.Scripts.GamePlay.FireExtinguisher
{
    public class FireController : MonoBehaviour
    {
        [SerializeField] private float initialIntensity = 5f;
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private TaskDefinition extinguishTask;

        private float intensity;

        public bool IsExtinguished => intensity <= 0f;

        private void Awake()
        {
            intensity = initialIntensity;
        }

        public void Extinguish(float amount)
        {
            if(IsExtinguished)
                return;
            
            intensity -= amount;
            
            if(intensity >= 0)
                fireParticles.transform.localScale = Vector3.one * (intensity / initialIntensity);
            else
            {
                intensity = 0f;
                fireParticles.gameObject.SetActive(false);
                
                FireGroupExecutor.Instance.NotifyFireExtinguished(this);
            }
        }
        
    }
}
using Project.Scripts.Core.Item;
using UnityEngine;

namespace Project.Scripts.GamePlay.FireExtinguisher
{
    public class Extinguisher : PickableItem
    {
        [SerializeField] private ParticleSystem foamParticles;
        [SerializeField] private AudioSource sprayAudio;
        [SerializeField] private Transform sprayOrigin;
        [SerializeField] private float sprayDistance = 1f;
        [SerializeField] private float sprayRadius = 1.5f;
        [SerializeField] private LayerMask fireMask;

        private bool isUsing;
        private readonly Collider[] hitColliders = new Collider[16];

        private void Update()
        {
            if (!isUsing || !HasPickup)
                return;

            ExtinguishFire();
        }

        public override void BeginUse()
        {
            if (isUsing)
                return;

            isUsing = true;
            
            foamParticles?.Play();
            sprayAudio?.Play();
        }

        public override void EndUse()
        {
            if (!isUsing)
                return;

            isUsing = false;

            foamParticles?.Stop();
            sprayAudio?.Stop();
        }

        private void ExtinguishFire()
        {
            if (!sprayOrigin)
                return;

            Vector3 center =
                sprayOrigin.position +
                sprayOrigin.forward * sprayDistance;

            var numColliders = Physics.OverlapSphereNonAlloc(center, sprayRadius, hitColliders, fireMask);
            for (var index = 0; index < numColliders; index++)
            {
                var hit = hitColliders[index];
                if (hit.TryGetComponent(out FireController fireController))
                    fireController.Extinguish(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            EndUse();
        }
    }
}
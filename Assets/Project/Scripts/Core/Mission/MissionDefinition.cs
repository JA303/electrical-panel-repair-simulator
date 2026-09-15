using UnityEngine;

namespace Project.Scripts.Core.Mission
{
    [CreateAssetMenu(menuName = "Mission/Mission")]
    public class MissionDefinition : ScriptableObject
    {
        [SerializeField] private PhaseDefinition[] phases;

        public PhaseDefinition[] Phases => phases;
    }
}
using UnityEngine;

namespace Project.Scripts.Core.Mission
{
    [CreateAssetMenu(menuName = "Mission/Mission Phase")]
    public class PhaseDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string title;
        [SerializeField] private TaskDefinition[] tasks;

        public string Id => id;
        public string Title => title;
        public TaskDefinition[] Tasks => tasks;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(id))
                id = name;
        }
#endif
    }
}
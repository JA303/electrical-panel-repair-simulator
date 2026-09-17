using System.Text.Json;
using Project.Scripts.Core.Item;
using UnityEngine;

namespace Project.Scripts.Core.Mission
{
    [CreateAssetMenu(menuName = "Mission/Mission Task")]
    public class TaskDefinition : ScriptableObject
    {
        [Header("Identity")] [SerializeField] private string id;
        [SerializeField] private string title;

        [Header("Execution")] [SerializeField] private TaskType type;
        [SerializeField] private RequiredHand requiredHand;
        [SerializeField] private bool lockPlayer = true;

        [Header("Requirements")] 
        [SerializeField] private ItemDefinition requiredItem;
        [SerializeField] private TaskDefinition[] prerequisites;

        [Header("UI")] [SerializeField] private bool showInTaskList = true;

        public string Id => id;
        public string Title => title;
        public TaskType Type => type;
        public RequiredHand RequiredHand => requiredHand;
        public bool LockPlayer => lockPlayer;
        public ItemDefinition RequiredItem => requiredItem;
        public TaskDefinition[] Prerequisites => prerequisites;
        public bool ShowInTaskList => showInTaskList;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(id))
                id = JsonNamingPolicy.SnakeCaseLower.ConvertName(title);;
        }
#endif
    }
}
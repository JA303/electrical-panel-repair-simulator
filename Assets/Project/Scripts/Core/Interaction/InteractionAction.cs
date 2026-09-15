using Project.Scripts.Core.Item;
using Project.Scripts.Core.Mission;

namespace Project.Scripts.Core.Interaction
{
    [System.Serializable]
    public class InteractionAction
    {
        public string label;
        public InteractionActionType actionType;

        public TaskDefinition task;
        public ItemDefinition requiredItem;

        public TaskActionExecutor executor;
    }
}
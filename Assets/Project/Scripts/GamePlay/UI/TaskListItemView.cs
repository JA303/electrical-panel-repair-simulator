using System.Text;
using TMPro;
using UnityEngine;
using Project.Scripts.Core.Mission;

namespace Project.Scripts.GamePlay.UI
{
    public class TaskListItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text statusText;

        private MissionRuntime missionRuntime;

        private void OnEnable()
        {
            MissionEvents.PhaseChanged += OnPhaseChanged;
            MissionEvents.TaskStatusChanged += OnTaskStatusChanged;
        }

        private void OnDisable()
        {
            MissionEvents.PhaseChanged -= OnPhaseChanged;
            MissionEvents.TaskStatusChanged -= OnTaskStatusChanged;
        }

        private void Start()
        {
            RefreshTaskList();
        }

        private void OnPhaseChanged(PhaseDefinition phase)
        {
            titleText.SetText(phase.Title);
            RefreshTaskList();
        }

        private void OnTaskStatusChanged(
            TaskDefinition changedTask,
            TaskStatus status)
        {
            if(status != TaskStatus.Completed)
                return;

            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            missionRuntime = MissionRuntime.Instance;
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (var task in missionRuntime.GetActivePhaseTasks())
            {
                if(!task.ShowInTaskList)
                    continue;
                
                if (missionRuntime.GetStatus(task) != TaskStatus.Pending) 
                    continue;
                
                if (missionRuntime.ArePrerequisitesCompleted(task))
                    stringBuilder.AppendLine($"-Task Name: {task.Title}");
            }
            
            RefreshTaskListText(stringBuilder.ToString());
        }

        private void RefreshTaskListText(string newTaskStatusText)
        {
            statusText.SetText(newTaskStatusText);
        }
    }
}
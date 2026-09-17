using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Core.Mission
{
    public class AutomaticTaskRunner : MonoBehaviour
    {
        [SerializeField] private TaskDefinition task;
        [SerializeField] private TaskActionExecutor executor;
        [SerializeField] private PlayerInteractor interactor;
        
        private bool hasStarted;

        private void OnEnable()
        {
            MissionEvents.AutomaticTaskAvailable += OnAutomaticTaskAvailable;
        }
        
        private void OnDisable()
        {
            MissionEvents.AutomaticTaskAvailable -= OnAutomaticTaskAvailable;
        }

        private void OnAutomaticTaskAvailable(TaskDefinition availableTask)
        {
            if(availableTask != task)
                return;
            TryRun();
        }

        private void TryRun()
        {
            if (hasStarted)
                return;

            if (!task || !executor)
                return;

            MissionRuntime mission = MissionRuntime.Instance;

            if (!mission)
                return;

            if (!mission.CanStart(task))
                return;

            hasStarted = true;
            executor.Execute(task, interactor);
        }
    }
}
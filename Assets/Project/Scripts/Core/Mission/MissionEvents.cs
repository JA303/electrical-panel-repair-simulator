using System;

namespace Project.Scripts.Core.Mission
{
    public static class MissionEvents
    {
        public static event Action<TaskDefinition, TaskStatus> TaskStatusChanged;
        public static event Action<PhaseDefinition> PhaseChanged;
        public static event Action MissionCompleted;

        public static void RaiseTaskStatusChanged(
            TaskDefinition task,
            TaskStatus status)
        {
            TaskStatusChanged?.Invoke(task, status);
        }

        public static void RaisePhaseChanged(PhaseDefinition phase)
        {
            PhaseChanged?.Invoke(phase);
        }

        public static void RaiseMissionCompleted()
        {
            MissionCompleted?.Invoke();
        }

        public static void Clear()
        {
            TaskStatusChanged = null;
            PhaseChanged = null;
            MissionCompleted = null;
        }
    }
}
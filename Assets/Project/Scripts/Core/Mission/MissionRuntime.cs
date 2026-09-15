using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Core.Mission
{
    public class MissionRuntime : MonoBehaviour
{
    public static MissionRuntime Instance { get; private set; }

    [SerializeField] private MissionDefinition missionDefinition;

    private readonly Dictionary<TaskDefinition, TaskStatus> taskStates = new();
    private readonly Dictionary<TaskDefinition, PhaseDefinition> taskPhases = new();

    private int activePhaseIndex;

    public PhaseDefinition ActivePhase
    {
        get
        {
            if (missionDefinition == null ||
                missionDefinition.Phases == null ||
                activePhaseIndex >= missionDefinition.Phases.Length)
            {
                return null;
            }

            return missionDefinition.Phases[activePhaseIndex];
        }
    }

    public bool IsMissionCompleted { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        BuildRuntimeState();
    }

    private void Start()
    {
        TryStartAutomaticTasks();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            MissionEvents.Clear();
    }

    private void BuildRuntimeState()
    {
        taskStates.Clear();
        taskPhases.Clear();

        if (missionDefinition == null)
        {
            Debug.LogError("MissionDefinition is not assigned.");
            return;
        }

        foreach (PhaseDefinition phase in missionDefinition.Phases)
        {
            if (phase == null || phase.Tasks == null)
                continue;

            foreach (TaskDefinition task in phase.Tasks)
            {
                if (task == null)
                    continue;

                if (taskStates.ContainsKey(task))
                {
                    Debug.LogError($"Duplicate task detected: {task.name}");
                    continue;
                }

                taskStates.Add(task, TaskStatus.Pending);
                taskPhases.Add(task, phase);
            }
        }
    }

    public TaskStatus GetStatus(TaskDefinition task)
    {
        if (task == null)
            return TaskStatus.Pending;

        return taskStates.TryGetValue(task, out TaskStatus status)
            ? status
            : TaskStatus.Pending;
    }

    public bool IsCompleted(TaskDefinition task)
    {
        return GetStatus(task) == TaskStatus.Completed;
    }

    public bool IsRunning(TaskDefinition task)
    {
        return GetStatus(task) == TaskStatus.Running;
    }

    public bool IsActivePhase(TaskDefinition task)
    {
        if (task == null || ActivePhase == null)
            return false;

        return taskPhases.TryGetValue(task, out PhaseDefinition phase)
               && phase == ActivePhase;
    }

    public bool ArePrerequisitesCompleted(TaskDefinition task)
    {
        if (task == null)
            return false;

        foreach (TaskDefinition prerequisite in task.Prerequisites)
        {
            if (prerequisite == null)
                continue;

            if (!IsCompleted(prerequisite))
                return false;
        }

        return true;
    }

    public bool CanStart(TaskDefinition task)
    {
        if (task == null || IsMissionCompleted)
            return false;

        if (!IsActivePhase(task))
            return false;

        if (GetStatus(task) != TaskStatus.Pending)
            return false;

        return ArePrerequisitesCompleted(task);
    }

    public bool TryStart(TaskDefinition task)
    {
        if (!CanStart(task))
            return false;

        taskStates[task] = TaskStatus.Running;
        MissionEvents.RaiseTaskStatusChanged(task, TaskStatus.Running);
        return true;
    }

    public bool TryComplete(TaskDefinition task)
    {
        if (task == null)
            return false;

        if (GetStatus(task) != TaskStatus.Running)
        {
            Debug.LogWarning(
                $"Task cannot complete because it is not running: {task.name}");
            return false;
        }

        taskStates[task] = TaskStatus.Completed;
        MissionEvents.RaiseTaskStatusChanged(task, TaskStatus.Completed);

        TryAdvancePhase();
        return true;
    }

    public void Cancel(TaskDefinition task)
    {
        if (task == null)
            return;

        if (GetStatus(task) != TaskStatus.Running)
            return;

        taskStates[task] = TaskStatus.Pending;
        MissionEvents.RaiseTaskStatusChanged(task, TaskStatus.Pending);
    }

    private void TryAdvancePhase()
    {
        if (ActivePhase == null)
            return;

        foreach (TaskDefinition task in ActivePhase.Tasks)
        {
            if (task == null)
                continue;

            if (GetStatus(task) != TaskStatus.Completed)
                return;
        }

        activePhaseIndex++;

        if (activePhaseIndex >= missionDefinition.Phases.Length)
        {
            IsMissionCompleted = true;
            MissionEvents.RaiseMissionCompleted();
            return;
        }

        MissionEvents.RaisePhaseChanged(ActivePhase);
        TryStartAutomaticTasks();
    }

    private void TryStartAutomaticTasks()
    {
        if (ActivePhase == null)
            return;

        foreach (TaskDefinition task in ActivePhase.Tasks)
        {
            if (task == null)
                continue;

            if (task.Type != TaskType.Automatic)
                continue;

            if (CanStart(task))
                MissionEvents.RaiseTaskStatusChanged(task, TaskStatus.Pending);
        }
    }

    public IEnumerable<TaskDefinition> GetActivePhaseTasks()
    {
        if (ActivePhase == null)
            yield break;

        foreach (TaskDefinition task in ActivePhase.Tasks)
        {
            if (task != null)
                yield return task;
        }
    }
}
}
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
    private readonly HashSet<TaskDefinition> taskPrerequisitesCompletedCache = new();

    private int activePhaseIndex;
    
    public bool IsMissionCompleted { get; private set; }

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
            if (!phase || phase.Tasks == null)
                continue;

            foreach (TaskDefinition task in phase.Tasks)
            {
                if (!task)
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
        return !task ? TaskStatus.Pending : taskStates.GetValueOrDefault(task, TaskStatus.Pending);
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
        if (!task || !ActivePhase)
            return false;

        return taskPhases.TryGetValue(task, out PhaseDefinition phase)
               && phase == ActivePhase;
    }

    public bool ArePrerequisitesCompleted(TaskDefinition task)
    {
        if (!task)
            return false;

        if (taskPrerequisitesCompletedCache.Contains(task))
            return true;

        foreach (TaskDefinition prerequisite in task.Prerequisites)
        {
            if (!prerequisite)
                continue;

            if (!IsCompleted(prerequisite))
                return false;
        }

        taskPrerequisitesCompletedCache.Add(task);
        return true;
    }

    public bool CanStart(TaskDefinition task)
    {
        if (!task || IsMissionCompleted)
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
        if (!task)
            return false;

        if (GetStatus(task) != TaskStatus.Running)
            return false;

        taskStates[task] = TaskStatus.Completed;

        MissionEvents.RaiseTaskStatusChanged(
            task,
            TaskStatus.Completed);

        if (IsCurrentPhaseCompleted())
            AdvancePhase();
        else
            TryStartAutomaticTasks();

        return true;
    }
    
    private bool IsCurrentPhaseCompleted()
    {
        if (!ActivePhase)
            return false;

        foreach (TaskDefinition task in ActivePhase.Tasks)
        {
            if (!task)
                continue;

            if (GetStatus(task) != TaskStatus.Completed)
                return false;
        }

        return true;
    }

    public void Cancel(TaskDefinition task)
    {
        if (!task)
            return;

        if (GetStatus(task) != TaskStatus.Running)
            return;

        taskStates[task] = TaskStatus.Pending;
        MissionEvents.RaiseTaskStatusChanged(task, TaskStatus.Pending);
    }

    private void AdvancePhase()
    {
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
        if (!ActivePhase)
            return;

        foreach (TaskDefinition task in ActivePhase.Tasks)
        {
            if (!task)
                continue;

            if (task.Type != TaskType.Automatic)
                continue;

            if (CanStart(task))
                MissionEvents.RaiseAutomaticTaskAvailable(task);
        }
    }

    public IEnumerable<TaskDefinition> GetActivePhaseTasks()
    {
        if (!ActivePhase)
            yield break;

        foreach (TaskDefinition task in ActivePhase.Tasks)
        {
            if (task)
                yield return task;
        }
    }
}
}
using System.Collections;
using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Core.Mission
{
    public abstract class TaskActionExecutor : MonoBehaviour
    {
        public void Execute(
            TaskDefinition task,
            PlayerInteractor interactor)
        {
            if (!task || !interactor)
                return;

            StartCoroutine(ExecuteRoutine(task, interactor));
        }

        private IEnumerator ExecuteRoutine(
            TaskDefinition task,
            PlayerInteractor interactor)
        {
            MissionRuntime mission = MissionRuntime.Instance;

            if (!mission || !mission.TryStart(task))
                yield break;

            object controlToken = null;

            if (interactor.ControlGate)
                controlToken = interactor.ControlGate.Acquire();

            bool success = false;

            yield return ExecuteAction(task, interactor, result =>
            {
                success = result;
            });

            if (success)
                mission.TryComplete(task);
            else
                mission.Cancel(task);

            interactor.ControlGate?.Release(controlToken);
        }

        protected abstract IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            System.Action<bool> completed);
    }
}
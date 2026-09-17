using System;
using System.Collections;
using Project.Scripts.Core.Mission;
using Project.Scripts.Player;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Scripts.GamePlay
{
    public class FireCutsceneExecuter : TaskActionExecutor
    {
        [SerializeField] private PlayableDirector director;

        private bool timelineFinished;

        protected override IEnumerator ExecuteAction(
            TaskDefinition task,
            PlayerInteractor interactor,
            Action<bool> completed)
        {
            if (!director)
            {
                completed(false);
                yield break;
            }

            timelineFinished = false;

            director.stopped += OnTimelineStopped;
            director.Play();
            var token = interactor.ControlGate.Acquire();

            yield return new WaitUntil(() => timelineFinished);
            
            interactor.ControlGate.Release(token);
            director.stopped -= OnTimelineStopped;

            completed(true);
        }

        private void OnTimelineStopped(PlayableDirector stoppedDirector)
        {
            if (stoppedDirector == director)
                timelineFinished = true;
#if UNITY_EDITOR
            Debug.Log("TimelineStopped");
#endif
        }
    }
}
using System.Collections;
using AugmentedReality.Poi;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantDefaultMemberInitializer

namespace AugmentedReality.Items
{
    public class ARItemStoryDefault : ARItemPoiDefault
    {
        [FormerlySerializedAs("startPlayableDirector")]
        [SerializeField]
        public PlayableDirector playerStart = null;

        protected Coroutine playCoroutine;

        public bool WaitForInput { get; protected set; } = false;

        public override void InitAndHide()
        {
            base.InitAndHide();
            StopPlayScript();
        }

        public override void ShowFullObject()
        {
            base.ShowFullObject();
            StartPlayScript();
        }

        protected void StartPlayScript()
        {
            if (playCoroutine != null) { StopPlayScript(); }

            playCoroutine = StartCoroutine(PlayScript());
        }

        protected virtual void StopPlayScript()
        {
            StopAllPlayableDirectors();

            if (playCoroutine != null) {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }
        }

        protected void StopAllPlayableDirectors()
        {
            var playableDirectors = GetComponentsInChildren<PlayableDirector>(true);
            foreach (var pd in playableDirectors) { pd.Stop(); }
        }

        protected virtual IEnumerator PlayScript()
        {
            playerStart.Play();
            while (playerStart.state != PlayState.Paused) { yield return null; }

            playCoroutine = null;
        }

        public StoryPoi RaycastStoryPoi(Ray ray, float distance = 10)
        {
            var basePoi = RaycastPoi(ray, distance);
            if (basePoi is StoryPoi storyPoi) { return storyPoi; }

            return null;
        }

        public virtual void StoryPoiClicked(StoryPoi storyPoi) { }
    }
}

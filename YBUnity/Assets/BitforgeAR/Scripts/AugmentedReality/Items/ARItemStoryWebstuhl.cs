using System.Collections;
using System.Collections.Generic;
using AugmentedReality.Poi;
using UnityEngine;
using UnityEngine.Playables;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable CommentTypo

namespace AugmentedReality.Items
{
    public class ARItemStoryWebstuhl : ARItemStoryDefault
    {
        [SerializeField]
        private StoryPoi triggerKeys = null;

        [SerializeField]
        private StoryPoi triggerUhr = null;

        [SerializeField]
        private StoryPoi triggerWebstuhl = null;

        [SerializeField]
        private StoryPoi triggerRapport = null;

        [SerializeField]
        private PlayableDirector playerKeys = null;

        [SerializeField]
        private PlayableDirector playerUhr = null;

        [SerializeField]
        private PlayableDirector playerWebstuhl = null;

        [SerializeField]
        private PlayableDirector playerRapport = null;

        [SerializeField]
        private PlayableDirector playerAbschluss = null;

        private readonly HashSet<StoryPoi> _poiPlayed = new HashSet<StoryPoi>();
        private PlayableDirector _actualPlayingDirector = null;

        private bool IsSomethingPlaying => _actualPlayingDirector != null && _actualPlayingDirector.state == PlayState.Playing;

        protected override IEnumerator PlayScript()
        {
            // Vorstellung
            playerStart.Play();
            while (playerStart.state != PlayState.Paused) { yield return null; }

            // Loop through all trigger clips
            while (!AllTriggersPlayed()) {
                WaitForInput = !IsSomethingPlaying;
                yield return null;
            }
            
            // wait until finished
            while (IsSomethingPlaying) {
                yield return null;
            }

            // Abschluss
            WaitForInput = false;
            playerAbschluss.Play();
            while (playerAbschluss.state != PlayState.Paused) { yield return null; }

            playCoroutine = null;
        }

        protected override void StopPlayScript()
        {
            base.StopPlayScript();
            WaitForInput = false;
            _actualPlayingDirector = null;
        }

        public override void StoryPoiClicked(StoryPoi storyPoi)
        {
            if (!IsSomethingPlaying && storyPoi) {
                _actualPlayingDirector = null;

                if (storyPoi == triggerKeys) { _actualPlayingDirector = playerKeys; }
                else if (storyPoi == triggerUhr) { _actualPlayingDirector = playerUhr; }
                else if (storyPoi == triggerWebstuhl) { _actualPlayingDirector = playerWebstuhl; }
                else if (storyPoi == triggerRapport) { _actualPlayingDirector = playerRapport; }


                // start
                if (_actualPlayingDirector != null) {
                    _poiPlayed.Add(storyPoi);
                    _actualPlayingDirector.Play();
                }
            }
        }

        private bool AllTriggersPlayed()
        {
            return _poiPlayed.Contains(triggerKeys) &&
                _poiPlayed.Contains(triggerUhr) &&
                _poiPlayed.Contains(triggerWebstuhl) &&
                _poiPlayed.Contains(triggerRapport);
        }
    }
}

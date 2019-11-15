using System.Collections;
using AugmentedReality.Poi;
using UnityEngine;
using UnityEngine.Playables;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable IdentifierTypo

namespace AugmentedReality.Items
{
    public class ARItemStoryTorfstecher : ARItemStoryDefault
    {
        [SerializeField]
        private StoryPoi triggerTorfspaten = null;

        [SerializeField]
        private StoryPoi triggerTorfsode = null;

        [SerializeField]
        private StoryPoi triggerMost = null;

        [SerializeField]
        private StoryPoi triggerSchubkarren = null;

        [SerializeField]
        private StoryPoi triggerSchnur = null;

        [SerializeField]
        private PlayableDirector playerTorfspaten = null;

        [SerializeField]
        private PlayableDirector playerSchubkarren = null;

        [SerializeField]
        private PlayableDirector playerMost = null;

        [SerializeField]
        private PlayableDirector playerTorfsode = null;

        [SerializeField]
        private PlayableDirector playerVerabschiedung = null;

        [SerializeField]
        private PlayableDirector playerFalsch = null;

        [SerializeField]
        private PlayableDirector playerRichtig = null;

        private StoryPoi _targetPoi;

        protected override IEnumerator PlayScript()
        {
            // 1 VorstellungUndSchnur
            playerStart.Play();
            while (playerStart.state != PlayState.Paused) { yield return null; }

            // 1 wait for Schnur input
            _targetPoi = triggerSchnur;
            while (_targetPoi != null) {
                WaitForInput = playerFalsch.state != PlayState.Playing;
                yield return null;
            }

            // 1 richtige antwort feiern
            if (playerRichtig != null) {
                playerRichtig.Play();
                while (playerRichtig.state != PlayState.Paused) { yield return null; }
            }

            // 2 Torfspaten
            playerTorfspaten.Play();
            while (playerTorfspaten.state != PlayState.Paused) { yield return null; }

            // 2 wait for Torfspaten input
            _targetPoi = triggerTorfspaten;
            while (_targetPoi != null) {
                WaitForInput = playerFalsch.state != PlayState.Playing;
                yield return null;
            }

            // 2 richtige antwort feiern
            if (playerRichtig != null) {
                playerRichtig.Play();
                while (playerRichtig.state != PlayState.Paused) { yield return null; }
            }

            // 3 Schubkarren
            playerSchubkarren.Play();
            while (playerSchubkarren.state != PlayState.Paused) { yield return null; }

            // 3 wait for Schubkarren input
            _targetPoi = triggerSchubkarren;
            while (_targetPoi != null) {
                WaitForInput = playerFalsch.state != PlayState.Playing;
                yield return null;
            }

            // 3 richtige antwort feiern
            if (playerRichtig != null) {
                playerRichtig.Play();
                while (playerRichtig.state != PlayState.Paused) { yield return null; }
            }

            // 4 Most
            playerMost.Play();
            while (playerMost.state != PlayState.Paused) { yield return null; }

            // 4 wait for Most input
            _targetPoi = triggerMost;
            while (_targetPoi != null) {
                WaitForInput = playerFalsch.state != PlayState.Playing;
                yield return null;
            }

            // 4 richtige antwort feiern
            if (playerRichtig != null) {
                playerRichtig.Play();
                while (playerRichtig.state != PlayState.Paused) { yield return null; }
            }

            // 5 Torfsode
            playerTorfsode.Play();
            while (playerTorfsode.state != PlayState.Paused) { yield return null; }

            // 5 wait for Torfsode input
            _targetPoi = triggerTorfsode;
            while (_targetPoi != null) {
                WaitForInput = playerFalsch.state != PlayState.Playing;
                yield return null;
            }

            // 5 richtige antwort feiern
            if (playerRichtig != null) {
                playerRichtig.Play();
                while (playerRichtig.state != PlayState.Paused) { yield return null; }
            }

            // 6 Verabschiedung
            playerVerabschiedung.Play();
            while (playerVerabschiedung.state != PlayState.Paused) { yield return null; }

            playCoroutine = null;
        }

        protected override void StopPlayScript()
        {
            base.StopPlayScript();
            WaitForInput = false;
            _targetPoi = null;
        }

        public override void StoryPoiClicked(StoryPoi storyPoi)
        {
            if (_targetPoi == null || playerFalsch.state == PlayState.Playing) { return; }

            if (storyPoi == _targetPoi) {
                // all done, go back to main story
                WaitForInput = false;
                _targetPoi = null;
            }
            else { playerFalsch.Play(); }
        }
    }
}

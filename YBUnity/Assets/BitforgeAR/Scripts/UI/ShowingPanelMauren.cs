using AugmentedReality.Items;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class ShowingPanelMauren : ShowingPanel
    {
        // Start is called before the first frame update
        [SerializeField]
        private RoundButton playButton = null;

        [SerializeField]
        private RoundButton rewindButton = null;

        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private Text subtitleText = null;

        private ARItemMauren _arItemMauren;
        private Tween _textTween;

        protected override void Awake()
        {
            base.Awake();

            _arItemMauren = FindObjectOfType<ARItemMauren>();

            playButton.OnClick.AddListener(TogglePlay);
            rewindButton.OnClick.AddListener(TogglePlay);
            titleText.text = string.Empty;
            subtitleText.text = string.Empty;
        }

        public override void Show()
        {
            UpdateUiBasedOnExperienceStep();
            base.Show();
        }

        public override void Hide()
        {
            playButton.Hide(false);
            rewindButton.Hide(false);
            base.Hide();
        }

        public override void TouchOnBlankScreen(Vector3 position)
        {
            base.TouchOnBlankScreen(position);
            TogglePlay();
        }

        protected override void PreCaptureScreenshot()
        {
            playButton.gameObject.SetActive(false);
            rewindButton.gameObject.SetActive(false);
            titleText.gameObject.SetActive(false);
            subtitleText .gameObject.SetActive(false);
          
            base.PreCaptureScreenshot();
        }

        protected override void PostCaptureScreenshot()
        {
            playButton.gameObject.SetActive(true);
            rewindButton.gameObject.SetActive(true);
            titleText.gameObject.SetActive(true);
            subtitleText .gameObject.SetActive(true);
            
            base.PostCaptureScreenshot();
        }

        private void TogglePlay()
        {
            _arItemMauren.ToggleExperienceStep();
            UpdateUiBasedOnExperienceStep();
        }

        private void UpdateUiBasedOnExperienceStep(bool immediate = false)
        {
            string title;
            string subtitle;

            if (_arItemMauren.ExperienceStep == 0) {
                playButton.Show();
                rewindButton.Hide(immediate);

                title = Localization.GetText("unity_ueberflutung_dammbruch_title");
                subtitle = Localization.GetText("unity_ueberflutung_dammbruch_subtitle");
            }
            else {
                playButton.Hide(immediate);
                rewindButton.Show();

                title = Localization.GetText("unity_ueberflutung_meineposition_title");
                subtitle = Localization.GetText("unity_ueberflutung_meineposition_subtitle");
            }

            _textTween?.Kill();
            if (immediate) {
                titleText.text = title;
                subtitleText.text = subtitle;
                titleText.color = Color.white;
                subtitleText.color = Color.white;
            }
            else {
                var sequence = DOTween.Sequence();
                sequence.Insert(0, titleText.DOText(title, 0.25f));
                sequence.Insert(0, titleText.DOFade(1, 0.25f));
                sequence.Insert(0.1f, subtitleText.DOText(subtitle, 0.25f));
                sequence.Insert(0.1f, subtitleText.DOFade(1, 0.25f));

                sequence.Insert(8f, titleText.DOFade(0, 0.25f));
                sequence.Insert(8f - 0.1f, subtitleText.DOFade(0, 0.25f));

                sequence.AppendCallback(
                    () =>
                    {
                        titleText.text = string.Empty;
                        subtitleText.text = string.Empty;
                    }
                );
                _textTween = sequence;
            }
        }
    }
}

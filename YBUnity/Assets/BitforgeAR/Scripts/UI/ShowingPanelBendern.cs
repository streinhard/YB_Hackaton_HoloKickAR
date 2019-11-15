using AugmentedReality.Items;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class ShowingPanelBendern : ShowingPanel
    {
        [SerializeField]
        private Slider timeslider = null;

        [SerializeField]
        private Text timeSliderInfoTitle = null;

        [SerializeField]
        private Text timeSliderInfoText = null;

        private ARItemBendern _arItemBendern;

        private int IntSliderValue => Mathf.RoundToInt(timeslider.value);

        private Tween _punchTextTween;

        protected override void Awake()
        {
            base.Awake();

            _arItemBendern = FindObjectOfType<ARItemBendern>();
            timeslider.onValueChanged.RemoveAllListeners();
            timeslider.onValueChanged.AddListener(TimeSliderValueChanged);
            timeslider.wholeNumbers = true;
            timeslider.minValue = 0;
            timeslider.maxValue = _arItemBendern.BuildingCount - 1;
        }

        protected override void Start()
        {
            base.Start();
            TimeSliderValueChanged(0);
        }

        protected override void PreCaptureScreenshot()
        {
            timeslider.gameObject.SetActive(false);
            timeSliderInfoTitle.gameObject.SetActive(false);
            timeSliderInfoText.gameObject.SetActive(false);

            base.PreCaptureScreenshot();
        }

        protected override void PostCaptureScreenshot()
        {
            timeslider.gameObject.SetActive(true);
            timeSliderInfoTitle.gameObject.SetActive(true);
            timeSliderInfoText.gameObject.SetActive(true);
            
            base.PostCaptureScreenshot();
        }

        public override void TouchOnBlankScreen(Vector3 position)
        {
            base.TouchOnBlankScreen(position);

            // go to next value
            var newValue = IntSliderValue + 1;
            if (newValue >= _arItemBendern.BuildingCount) { newValue = 0; }

            timeslider.value = newValue;
        }

        private void TimeSliderValueChanged(float newValue)
        {
            var newIntValue = Mathf.RoundToInt(newValue);

            _arItemBendern.SetBuilding(newIntValue);

            // set text label
            PunchTextInfo(
                Localization.GetText(_arItemBendern.CurrentBuildingTitleKey),
                Localization.GetText(_arItemBendern.CurrentBuildingTextKey)
            );
        }

        private void PunchTextInfo(string title, string text)
        {
            // turn off all animations on info text
            _punchTextTween?.Kill();

            var sequence = DOTween.Sequence();

            timeSliderInfoTitle.text = title;
            //sequence.Insert(0f, timeSliderInfoTitle.DOText(title, 0.1f));
            sequence.Insert(0f, timeSliderInfoTitle.DOFade(1, 0.25f));

            sequence.Insert(0.1f, timeSliderInfoText.DOText(text, 0.4f));
            sequence.Insert(0.1f, timeSliderInfoText.DOFade(1, 0.25f));

            sequence.AppendInterval(5);

            sequence.Append(timeSliderInfoText.DOFade(0, 0.1f));
            sequence.Append(timeSliderInfoTitle.DOFade(0, 0.25f));

            sequence.OnComplete(
                () =>
                {
                    timeSliderInfoTitle.text = string.Empty;
                    timeSliderInfoText.text = string.Empty;
                }
            );

            _punchTextTween = sequence;
        }
    }
}

using AugmentedReality;
using AugmentedReality.Items;
using AugmentedReality.Poi;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class ShowingPanelInteractionPoint : ShowingPanel
    {
        [SerializeField]
        private RoundButton actionButton = null;

        [SerializeField]
        private Text info = null;

        private ARItemPoiDefault _arItemInteractionPoints = null;
        private Camera _camera = null;
        private Transform _cameraTransform = null;
        private InteractionPoint _selectedInteractionPoint = null;

        protected override void Start()
        {
            _arItemInteractionPoints = FindObjectOfType<ARItemPoiDefault>();

            _camera = ARControllerStackLoader.GetArCamera();
            _cameraTransform = _camera.transform;

            //actionButton.OnClick.RemoveAllListeners();
            //actionButton.OnClick.AddListener(PoiActionClicked);
            info.text = string.Empty;
            info.color = new Color(1, 1, 1, 0);
            
            base.Start();
        }

        public override void Show()
        {
            base.Show();
            //arrowHideShowButton.Show();
        }
        
        private void Update()
        {
            var cameraRay = new Ray(_cameraTransform.position, _cameraTransform.forward);
            var gazedBasePoi = _arItemInteractionPoints.RaycastPoi(cameraRay);

            if (!ReferenceEquals(gazedBasePoi, null) && gazedBasePoi is InteractionPoint interactionPoint) { PoiGazedOn(interactionPoint); }
            else { PoiGazedOff(); }
        }

        protected override void PreCaptureScreenshot()
        {
            base.PreCaptureScreenshot();
            actionButton.gameObject.SetActive(false);
            info.gameObject.SetActive(false);
            _arItemInteractionPoints.HidePois(true);
        }

        protected override void PostCaptureScreenshot()
        {
            base.PostCaptureScreenshot();
            actionButton.gameObject.SetActive(true);
            info.gameObject.SetActive(true);
            _arItemInteractionPoints.ShowPois();
        }

        public override void TouchOnBlankScreen(Vector3 position)
        {
            base.TouchOnBlankScreen(position);

            // just send a click if user already is gazing one
            if (_selectedInteractionPoint != null) {
                PoiActionClicked();
                return;
            }

            PoiActionClicked();
            var touchRay = _camera.ScreenPointToRay(position);
            var gazedBasePoi = _arItemInteractionPoints.RaycastPoi(touchRay);
            if (!ReferenceEquals(gazedBasePoi, null) && gazedBasePoi is InteractionPoint interactionPoint) {
                PoiGazedOn(interactionPoint);
                PoiActionClicked();
            }
        }

        private void PoiActionClicked()
        {
            if (_selectedInteractionPoint != null) {
                _arItemInteractionPoints.HidePois(false);
                OnPanoramaRequested?.Invoke(_selectedInteractionPoint);
            }
        }

        private void PoiGazedOn(InteractionPoint interactionPoint)
        {
            // return if already selected
            if (ReferenceEquals(interactionPoint, _selectedInteractionPoint)) { return; }

            // if there is already another poi selected
            // ReSharper disable once UseNullPropagation
            if (!ReferenceEquals(_selectedInteractionPoint, null)) { _selectedInteractionPoint.GazeOff(); }

            // select the new one
            _selectedInteractionPoint = interactionPoint;
            _selectedInteractionPoint.GazeOn();
            info.text = Localization.GetText(_selectedInteractionPoint.titleTextKey);

            DOTween.Kill(info);
            info.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            actionButton.Show();
        }

        private void PoiGazedOff()
        {
            if (!ReferenceEquals(_selectedInteractionPoint, null)) {
                _selectedInteractionPoint.GazeOff();
                _selectedInteractionPoint = null;

                DOTween.Kill(info);
                info.DOFade(0, 0.25f).SetEase(Ease.InCubic);
                actionButton.Hide(false);
            }
        }

        private void ArrowHideShowClicked()
        {
            _arItemInteractionPoints.TogglePoiVisibility();
        }
    }
}

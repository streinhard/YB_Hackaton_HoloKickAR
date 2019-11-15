using AugmentedReality.Items;
using UnityEngine;
// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class ShowingPanelBuilder : ShowingPanel
    {
        [SerializeField]
        private RoundButton placeButton = null;

        [SerializeField]
        private RoundButton deleteButton = null;

        [SerializeField]
        private RoundButton modePaintButton = null;

        [SerializeField]
        private RoundButton modeDeleteButton = null;

        [SerializeField]
        private RoundButton brickSelectionButton = null;

        private ARItemBuilder _arItemBuilder;
        private bool _actionButtonIsShown;

        protected override void Start()
        {
            _arItemBuilder = FindObjectOfType<ARItemBuilder>();

            placeButton.OnClick.RemoveAllListeners();
            placeButton.OnClick.AddListener(PlaceButtonClicked);

            deleteButton.OnClick.RemoveAllListeners();
            deleteButton.OnClick.AddListener(DeleteButtonClicked);

            modePaintButton.OnClick.RemoveAllListeners();
            modePaintButton.OnClick.AddListener(ModeButtonClicked);

            modeDeleteButton.OnClick.RemoveAllListeners();
            modeDeleteButton.OnClick.AddListener(ModeButtonClicked);

            brickSelectionButton.OnClick.RemoveAllListeners();
            brickSelectionButton.OnClick.AddListener(BrickSelectionButtonClicked);

            base.Start();
        }

        public override void Show()
        {
            base.Show();
            UpdateMode(true);
        }

        public override void Hide()
        {
            base.Hide();
            modePaintButton.Hide(true);
            deleteButton.Hide(true);
            modeDeleteButton.Hide(true);
            brickSelectionButton.Hide(true);
            placeButton.Hide(true);

            _actionButtonIsShown = false;
        }

        protected override void PreCaptureScreenshot()
        {
            placeButton.gameObject.SetActive(false);
            deleteButton .gameObject.SetActive(false);
            modePaintButton.gameObject.SetActive(false);
            modeDeleteButton.gameObject.SetActive(false);
            brickSelectionButton.gameObject.SetActive(false);
            _arItemBuilder.BasePlate.SetActive(false);
            base.PreCaptureScreenshot();
        }

        protected override void PostCaptureScreenshot()
        {
            placeButton.gameObject.SetActive(true);
            deleteButton .gameObject.SetActive(true);
            modePaintButton.gameObject.SetActive(true);
            modeDeleteButton.gameObject.SetActive(true);
            brickSelectionButton.gameObject.SetActive(true);
            _arItemBuilder.BasePlate.SetActive(true);
            base.PostCaptureScreenshot();
        }

        private void Update()
        {
            if (_arItemBuilder.IsInPlaceMode) {
                var showPlaceButton = _arItemBuilder.IsInPlaceMode && _arItemBuilder.CanPlaceObject;
                if (showPlaceButton != _actionButtonIsShown) {
                    if (showPlaceButton) { placeButton.Show(); }
                    else { placeButton.Hide(false); }

                    _actionButtonIsShown = showPlaceButton;
                }
            }
            else {
                var showDeleteButton = !_arItemBuilder.IsInPlaceMode && _arItemBuilder.CanDeleteObject;
                if (showDeleteButton != _actionButtonIsShown) {
                    if (showDeleteButton) { deleteButton.Show(); }
                    else { deleteButton.Hide(false); }

                    _actionButtonIsShown = showDeleteButton;
                }
            }
        }

        private void UpdateMode(bool immediate = false)
        {
            if (_arItemBuilder.IsInPlaceMode) {
                modePaintButton.Hide(immediate);
                deleteButton.Hide(immediate);
                modeDeleteButton.Show();
                brickSelectionButton.Show();
            }
            else {
                brickSelectionButton.Hide(immediate);
                modeDeleteButton.Hide(immediate);
                placeButton.Hide(immediate);
                modePaintButton.Show();
            }
        }

        private void PlaceButtonClicked()
        {
            _arItemBuilder.PlaceBrick();
        }

        private void DeleteButtonClicked()
        {
            _arItemBuilder.DeleteBrick();
        }

        private void BrickSelectionButtonClicked()
        {
            if (_arItemBuilder.IsInPlaceMode) { _arItemBuilder.NextBrick(); }
        }

        private void ModeButtonClicked()
        {
            _arItemBuilder.SwitchMode();
            _actionButtonIsShown = false;
            UpdateMode();
        }

        public override void TouchOnBlankScreen(Vector3 position)
        {
            base.TouchOnBlankScreen(position);

            if (_arItemBuilder.IsInPlaceMode) { PlaceButtonClicked(); }
            else { DeleteButtonClicked(); }
        }
    }
}

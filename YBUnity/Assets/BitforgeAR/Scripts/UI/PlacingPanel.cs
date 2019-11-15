using UnityEngine;
using UnityEngine.UI;
// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class PlacingPanel : GenericPanel
    {
        [SerializeField]
        private RoundButton placeButton = null;

        [SerializeField]
        private RoundButton scaleButton = null;

        public Button.ButtonClickedEvent OnPlaceClick => placeButton.OnClick;
        public Button.ButtonClickedEvent OnScaleClick => scaleButton.OnClick;

        public override void Hide()
        {
            base.Hide();
            HideButtons(true);
        }

        public void ShowButtons()
        {
            placeButton.Show();
            scaleButton.Show();
        }

        public void HideButtons(bool immediate)
        {
            placeButton.Hide(immediate);
            scaleButton.Hide(immediate);
        }

        public override void TouchOnBlankScreen(Vector3 position)
        {
            base.TouchOnBlankScreen(position);
            placeButton.OnClick.Invoke();
        }
    }
}

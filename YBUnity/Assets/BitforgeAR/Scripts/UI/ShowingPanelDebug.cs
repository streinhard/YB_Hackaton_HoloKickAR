using System.Text;
using AugmentedReality;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowingPanelDebug : ShowingPanel
    {
        public GameObject mainLightSource;
        public Text console;

        public Button orientationToggleBtn;
        public Button lightingToggleBtn;
        public Button fullScreenModeToggleBtn;
        public Button fullScreenToggleBtn;

        private LightEstimation _lightEstimation;
        private Light _lightComp;
        private readonly StringBuilder _sb = new StringBuilder();

        protected override void Start()
        {
            lightingToggleBtn.onClick.AddListener(ToggleLightingSetup);
            orientationToggleBtn.onClick.AddListener(ToggleOrientation);
            fullScreenModeToggleBtn.onClick.AddListener(ToggleFullScreenMode);
            fullScreenToggleBtn.onClick.AddListener(ToggleFullScreen);

            _lightEstimation = mainLightSource.GetComponent<LightEstimation>();
            _lightComp = mainLightSource.GetComponent<Light>();

            base.Start();
        }

        private void Update()
        {
            _sb.Clear();

            _sb.AppendLine("-----------------------");

            var safeArea = Screen.safeArea;

            _sb.AppendLine("Safe Area X: " + safeArea.x);
            _sb.AppendLine("Safe Area Y: " + safeArea.y);
            _sb.AppendLine("Safe Area Width: " + safeArea.width);
            _sb.AppendLine("Safe Area Height: " + safeArea.height);

            _sb.AppendLine("-----------------------");
            
            _sb.AppendLine("Screen W" + Screen.width);
            _sb.AppendLine("Screen H: " + Screen.height);
            
            _sb.AppendLine("-----------------------");
            /*
            _sb.AppendLine($"lightEstimation Brightness: {_lightEstimation.Brightness}");
            _sb.AppendLine($"lightEstimation ColorTemperature: {_lightEstimation.ColorTemperature}");
            _sb.AppendLine($"lightEstimation ColorCorrection: {_lightEstimation.ColorCorrection}");

            _sb.AppendLine("-----------------------");

            _sb.AppendLine($"light intensity: {_lightComp.intensity}");
            _sb.AppendLine($"light shadows: {_lightComp.shadows}");
             */
            _sb.AppendLine("-----------------------");

            _sb.AppendLine($"orientation: {Screen.orientation}");

            _sb.AppendLine("-----------------------");

            _sb.AppendLine($"fullScreenMode: {Screen.fullScreenMode}");
         
            console.text = _sb.ToString();
            
        }



        private void ToggleOrientation()
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;

            if (Screen.orientation == ScreenOrientation.Portrait) {
                Screen.orientation = ScreenOrientation.AutoRotation; 
            }
            else 
            {
                Screen.orientation = ScreenOrientation.Portrait;  
            }
        }

        private bool _toggleL;

        private void ToggleLightingSetup()
        {
            if (_toggleL) {
                _toggleL = !_toggleL;
                _lightEstimation.mixEstimation = 0f;
            }
            else {
                _toggleL = !_toggleL;
                _lightEstimation.mixEstimation = 0.5f;
            }
        }

        private void ToggleFullScreenMode()
        {
            switch (Screen.fullScreenMode) {
                case FullScreenMode.ExclusiveFullScreen:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case FullScreenMode.FullScreenWindow:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
                case FullScreenMode.MaximizedWindow:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case FullScreenMode.Windowed:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                default:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
            }
        }

        private void ToggleFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}

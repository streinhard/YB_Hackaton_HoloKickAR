//#define DEBUG_IX_PORTRAIT
//#define DEBUG_IX_LANDSCAPE
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable IdentifierTypo

namespace UI
{
    public class SafeAreaRect : MonoBehaviour
    {
        private bool _safeAreaChanged;
        private Vector2 _lastScreenSize;
        private Rect _lastSafeArea;
        private RectTransform _thisRectTransform;
        private CanvasScaler _canvasScaler;

        public Rect LastSafeArea => _lastSafeArea;

        private void Awake()
        {
            _canvasScaler = GetComponentInParent<CanvasScaler>();
            _thisRectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateSafeArea();
        }

        private void UpdateSafeArea()
        {
            #if DEBUG_IX_PORTRAIT
                var safeArea = new Rect(0,102, 1125,2202); 
                var screenSize = new Size(1125, 2436);
            #elif DEBUG_IX_LANDSCAPE
                var safeArea = new Rect(132, 63, 2172,1062); 
                var screenSize = new Size(2436, 1125);
            #else
                var safeArea = Screen.safeArea;
                var screenSize = new Vector2(Screen.width, Screen.height);
            #endif
            
            if (_lastSafeArea != safeArea || _lastScreenSize != screenSize)
            {
                ApplySafeArea(safeArea, screenSize);
            }
        }

        private void ApplySafeArea(Rect safeArea, Vector2 screenSize)
        {
            var scaleRatio = _canvasScaler.referenceResolution.x / screenSize.x;

            if (Screen.orientation == ScreenOrientation.Landscape)
            {
                scaleRatio = _canvasScaler.referenceResolution.y / screenSize.x;
            }

            var top = screenSize.y - safeArea.height - safeArea.y;
            var bottom = safeArea.y;

            var left = 0f;
            var right = safeArea.x;
            
            // when notch is on the left side
            if (Screen.orientation == ScreenOrientation.LandscapeLeft) 
            {
                left = safeArea.x;
                right = 0;
            }

            top *= scaleRatio;
            bottom *= scaleRatio;
            left *= scaleRatio;
            right *= scaleRatio;

            // no negative values
            top = Mathf.Max(0, top);
            bottom = Mathf.Max(0, bottom);
            left = Mathf.Max(0, left);
            right = Mathf.Max(0, right);

            _thisRectTransform.offsetMin = new Vector2(left, bottom);
            _thisRectTransform.offsetMax = new Vector2(-right, -top);
            
            _lastSafeArea = safeArea;
            _lastScreenSize =screenSize;
        }
    }
}
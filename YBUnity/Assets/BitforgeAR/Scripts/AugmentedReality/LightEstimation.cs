using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AugmentedReality {
    /// <summary>
    /// A component that can be used to access the most
    /// recently received light estimation information
    /// for the physical environment as observed by an
    /// AR device.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class LightEstimation : MonoBehaviour
    {
        /// <summary>
        /// The estimated brightness of the physical environment, if available.
        /// </summary>
        public float? Brightness { get; private set; }

        /// <summary>
        /// The estimated color temperature of the physical environment, if available.
        /// </summary>
        public float? ColorTemperature { get; private set; }

        /// <summary>
        /// The estimated color correction value of the physical environment, if available.
        /// </summary>
        public Color? ColorCorrection { get; private set; }

        [Range(0, 1)]
        public float mixEstimation = 1f;
        
        private Light _light;
        private float _originalIntensity;
        private float _originalTemperature;
        private Color _originalColor;

        private ARCameraManager _cameraManager;

        void Awake ()
        {
            _light = GetComponent<Light>();
            _originalIntensity = _light.intensity;
            _originalTemperature = _light.colorTemperature;
            _originalColor = _light.color;
            _cameraManager = FindObjectOfType<ARCameraManager>();
        }

        void OnEnable()
        {
            if (!Application.isEditor) { _cameraManager.frameReceived += FrameChanged; }
        }

        void OnDisable()
        {
            if (!Application.isEditor) { _cameraManager.frameReceived -= FrameChanged; }
        }

        void FrameChanged(ARCameraFrameEventArgs args)
        {
            mixEstimation = Mathf.Clamp01(mixEstimation);
            var oneMinusMix = Mathf.Clamp01(1f - mixEstimation);
            
            if (args.lightEstimation.averageBrightness.HasValue)
            {
                //Debug.Log($"averageBrightness: {args.lightEstimation.averageBrightness.Value}" );
                Brightness = args.lightEstimation.averageBrightness.Value;
                _light.intensity = mixEstimation * Brightness.Value + oneMinusMix * _originalIntensity;
            }

            if (args.lightEstimation.averageColorTemperature.HasValue)
            {
                //Debug.Log($"averageColorTemperature: {args.lightEstimation.averageColorTemperature.Value}" );
                ColorTemperature = args.lightEstimation.averageColorTemperature.Value;
                _light.colorTemperature = mixEstimation * ColorTemperature.Value + oneMinusMix * _originalTemperature;
            }
        
            if (args.lightEstimation.colorCorrection.HasValue)
            {
                //Debug.Log($"colorCorrection: {args.lightEstimation.colorCorrection.Value}" );
                ColorCorrection = args.lightEstimation.colorCorrection.Value;
                _light.color = Color.Lerp(_originalColor,ColorCorrection.Value,  mixEstimation);
            }
        }
    }
}

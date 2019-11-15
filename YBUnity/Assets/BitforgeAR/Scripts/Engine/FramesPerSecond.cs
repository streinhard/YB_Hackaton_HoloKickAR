using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Engine {
    public class FramesPerSecond : MonoBehaviour
    {
        public Text fpsText;

        // Attach this to a GUIText to make a frames/second indicator.
        //
        // It calculates frames/second over each updateInterval,
        // so the display does not keep changing wildly.
        //
        // It is also fairly accurate at very low FPS counts (<10).
        // We do this not by simply counting frames per interval, but
        // by accumulating FPS for each frame. This way we end up with
        // correct overall FPS even if the interval renders something like
        // 5.5 frames.

        public float updateInterval = 0.5F;
        public bool hideInReleaseBuild = true;

        private float _accumulated; // FPS accumulated over the interval
        private int _frames; // Frames drawn over the interval
        private float _timeLeftover; // Leftover time for current interval
        private MyLogHandler _myLogHandler;

        private void Awake()
        {
            if (hideInReleaseBuild) {
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _timeLeftover = updateInterval;
            //_myLogHandler= new MyLogHandler( Debug.unityLogger.logHandler);
            //Debug.unityLogger.logHandler = _myLogHandler;
        }

        private void Update()
        {
            _timeLeftover -= Time.deltaTime;
            _accumulated += Time.timeScale / Time.deltaTime;
            ++_frames;

            // Interval ended - update GUI text and start new interval
            if (_timeLeftover <= 0.0) {
                
                // display two fractional digits (f2 format)
                var fps = _accumulated / _frames;
                _timeLeftover = updateInterval;
                _accumulated = 0.0F;
                _frames = 0;
                
                if (fps < 10) {  fpsText.color = Color.red; }
                else if (fps < 30) { fpsText.color = Color.yellow; }
                else { fpsText.color = Color.green; }

                fpsText.text = $"{fps:F1} FPS\n";// + _myLogHandler.GetLog();
            }
        }

    }
    
    public class MyLogHandler : ILogHandler
    {
        private StringBuilder _builder = new StringBuilder();
        private ILogHandler _childLogHandler;

        public string GetLog()
        {
            return _builder.ToString();
        }
        
        public MyLogHandler(ILogHandler childLogHandler)
        {
            _childLogHandler = childLogHandler;
        }
        
        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            _childLogHandler.LogFormat(logType, context, format, args);
            _builder.AppendFormat(format, args);
        }

        public void LogException(Exception exception, Object context)
        {
            _childLogHandler.LogException(exception, context);
            _builder.AppendLine(exception.Message);
        }
    }
}
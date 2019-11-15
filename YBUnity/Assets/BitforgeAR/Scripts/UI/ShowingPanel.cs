using System.Collections;
using System.IO;
using AugmentedReality.Poi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class ShowingPanel : GenericPanel
    {
        private const string SCREENSHOT_FILE_NAME = "listory_screenshot.png";
        
        [SerializeField]
        private Button resetButton = null;

        [SerializeField]
        private Button photoButton = null;

        public PanoramaRequestEvent OnPanoramaRequested { get; } = new PanoramaRequestEvent();
        public Button.ButtonClickedEvent OnResetClick => resetButton.onClick;
        
        protected override void Awake()
        {
            if (photoButton != null) { photoButton.onClick.AddListener(OnShareScreenShot); }
            if (resetButton != null) { resetButton.onClick.AddListener(OnReset); }
            base.Awake();
        }

        private void OnShareScreenShot()
        {
            Debug.Log("TAKING SCREENSHOT");
            StartCoroutine(ShareScreenshotCoroutine());
        }

        private void OnReset()
        {
            //TODO go back to placement?
            //ArDirector.
        }

        private IEnumerator ShareScreenshotCoroutine()
        {
            var filePath = Path.Combine(Application.persistentDataPath, SCREENSHOT_FILE_NAME);
            
            // delete last screenshot if there was one
            if (File.Exists(filePath)) File.Delete(filePath);

            // prepare for screenshot
            PreCaptureScreenshot();
            
            // wait so that all ui components are disabled for sure
            yield return new WaitForEndOfFrame();
            
            #if !UNITY_EDITOR
            // capture new screenshot
            ScreenCapture.CaptureScreenshot(SCREENSHOT_FILE_NAME);
            #else
            yield return new WaitForSeconds(5);
            #endif
            // undo screenshot preparation
            PostCaptureScreenshot();
            
            #if !UNITY_EDITOR
            // wait while screenshot file is unavailable
            Debug.Log($"Waiting for photo: {Time.realtimeSinceStartup} @ {filePath}");
            var watchDogStart = Time.realtimeSinceStartup;
            while(IsFileUnavailable(filePath) && (Time.realtimeSinceStartup - watchDogStart) < 10f) {
                Debug.Log($"Waiting for photo: {Time.realtimeSinceStartup} @ {Time.realtimeSinceStartup - watchDogStart}");
                yield return new WaitForSeconds(.05f);
            }

            // share without text for the moment
            new NativeShare().AddFile(filePath).Share();
            #endif
        }

        protected virtual void PreCaptureScreenshot()
        {
            resetButton.gameObject.SetActive(false);
            photoButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
        }
        
        protected virtual void PostCaptureScreenshot()
        {
            resetButton.gameObject.SetActive(true);
            photoButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(true);
        }
 
        #if !UNITY_EDITOR
        private static bool IsFileUnavailable(string path)
        {
            // if file doesn't exist, return true
            if (!File.Exists(path))
                return true;
 
            var file = new FileInfo(path);
            FileStream stream = null;
 
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is: still being written to
                //or being processed by another thread or does not exist (has already been processed)
                return true;
            }
            finally {
                stream?.Close();
            }
 
            //file is not locked
            return false;
        }
        #endif
    }
    
    public class PanoramaRequestEvent : UnityEvent<InteractionPoint> { }
}

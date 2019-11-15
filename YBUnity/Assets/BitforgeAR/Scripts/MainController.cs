using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    private const string ROOT_SCENE_NAME = "PWA_Root";

    public GameObject uiCamera;
    public ListoryUiAnimation listoryUiAnimation;
    public float maxPwaLoadingTime = 5f;

    private ArAvailabilityChecker _arAvailabilityChecker;
    private bool _webViewIsReady;

    private float _watchDogTimer;
    private string _lastActiveSceneName;
    private IEnumerator _launchCoroutine;

    private void Awake()
    {
        _arAvailabilityChecker = GetComponent<ArAvailabilityChecker>();
    }

    private IEnumerator Start()
    {
        // Start AR availablility check
        _arAvailabilityChecker.StartCheck();

        OrientationController.LockOrientations();

        var introAnimation = listoryUiAnimation.CreateIntroAnimation();

        yield return introAnimation.WaitForCompletion();

        if (_webViewIsReady) {
            QualityController.SlowDownUnity();
            listoryUiAnimation.blackCurtain.SetActive(true);
            listoryUiAnimation.enabled = false;
            yield break;
        }

        var spinnerStarted = false;

        // if it takes more than 5 seconds load webview anyway
        while (!_webViewIsReady && _watchDogTimer < maxPwaLoadingTime) {
            if (!spinnerStarted) {
                listoryUiAnimation.CreateSpinnerAnimation();
                spinnerStarted = true;
            }

            _watchDogTimer += Time.deltaTime;
            yield return null;
        }

        QualityController.SlowDownUnity();
        listoryUiAnimation.blackCurtain.SetActive(true);
        listoryUiAnimation.enabled = false;
        
        yield return new WaitForSeconds(1);
        uiCamera.SetActive(false);
    }

    private void LaunchArScene(string arSceneName)
    {
        // return if there is already a ar launch happening
        if (_launchCoroutine != null) { return; }

        // return if an ar scene is active an loaded
        // ar scene launch is allowed on top of root scene
        var activeScene = SceneManager.GetActiveScene();
        if (!string.IsNullOrWhiteSpace(activeScene.name) && string.Equals(activeScene.name, ROOT_SCENE_NAME)) {
            _launchCoroutine = LaunchScene(arSceneName);
            StartCoroutine(_launchCoroutine);
        }
    }

    private IEnumerator LaunchScene(string sceneName)
    {
        // prepare loading
        AsyncOperation loadTask = null;

        QualityController.SpeedUpUnity();

        uiCamera.SetActive(true);
        var cam = uiCamera.GetComponent<Camera>();
        cam.cullingMask = 0;
        cam.backgroundColor = Color.black;
        if (listoryUiAnimation.gameObject.activeSelf) {
            listoryUiAnimation.gameObject.SetActive(false);
            //uiCamera.SetActive(false);
        }

        #if PLATFORM_ANDROID
        PermissionHandler.RequestCameraPermission();

        if(!PermissionHandler.HasCameraPermission()){
            yield return new WaitForSecondsRealtime(.1f); // wait once (unity is in background)
        }

        if(!PermissionHandler.HasCameraPermission())
        {
            //go back to Web
            _launchCoroutine = null;
            ShutdownArScene();
            yield break;
        }
        #endif

        try {
            // load ar scene asynchronously
            loadTask = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        catch (Exception ex) {
            // ignored
            Debug.LogException(ex);
        }

        // wait till loaded
        while (loadTask != null && !loadTask.isDone) { yield return null; }
        uiCamera.SetActive(false);

        // get scene by name
        try { SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); }
        catch (Exception ex) {
            // ignored
            Debug.LogException(ex);
        }

        yield return new WaitForEndOfFrame();

        try {

            // unlock orientation
            OrientationController.UnlockOrientations();
        }
        catch (Exception ex) {
            // ignored
            Debug.LogException(ex);
        }

        _launchCoroutine = null;
    }

    public void ShutdownArScene()
    {
        if (_launchCoroutine != null) { return; }

        _launchCoroutine = UnloadScene();
        StartCoroutine(_launchCoroutine);
        Debug.Log("close");
        
    }

    private IEnumerator UnloadScene()
    {
        // lock screen to portrait
        try { OrientationController.LockOrientations(); }
        catch (Exception ex) {
            // ignored
            Debug.Log("Exception: " + ex.Message);
        }

        // there was once a webview half of the screen
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForEndOfFrame();

        // unload ar scene and wait until scene is unloaded (not actually necessary)
        var unloadTasks = new List<AsyncOperation>();
        try {
            for (var i = 0; i < SceneManager.sceneCount; i++) {
                var tmpScene = SceneManager.GetSceneAt(i);
                if (!string.Equals(tmpScene.name, ROOT_SCENE_NAME, StringComparison.OrdinalIgnoreCase)) {
                    unloadTasks.Add(SceneManager.UnloadSceneAsync(tmpScene));
                }
            }
        }
        catch (Exception ex) {
            // ignored
            Debug.Log("Exception: " + ex.Message);
        }

        while (unloadTasks.Count > 0) {
            for (var i = unloadTasks.Count - 1; i >= 0; i--) {
                var unloadTask = unloadTasks[i];
                if (unloadTask == null || unloadTask.isDone) { unloadTasks.RemoveAt(i); }
            }

            yield return null;
        }

        yield return new WaitForEndOfFrame();

        // set root scene as active scene
        try { SceneManager.SetActiveScene(SceneManager.GetSceneByName(ROOT_SCENE_NAME)); }
        catch (Exception ex) {
            // ignored
            Debug.Log("Exception: " + ex.Message);
        }

        // turn down quality
        try { QualityController.SlowDownUnity(); }
        catch (Exception ex) {
            // ignored
            Debug.Log("Exception: " + ex.Message);
        }

        // ready for more launch action ;)
        _launchCoroutine = null;
    }

    private bool RootSceneIsActive()
    {
        return string.Equals(ROOT_SCENE_NAME, SceneManager.GetActiveScene().name, StringComparison.OrdinalIgnoreCase);
    }

    #region webview event handlers

    private void WebViewLoaded(string data)
    {
        _webViewIsReady = true;
    }

    private static void UserLocationPermissionRequest(string data)
    {
        PermissionHandler.RequestLocationPermission();
    }

    private static void SetLanguage(string data)
    {
        if (!string.IsNullOrWhiteSpace(data) && data.IndexOf("en", StringComparison.OrdinalIgnoreCase) >= 0) {
            Localization.IsGerman = false;
        }
        else { Localization.IsGerman = true; }
    }

    private static void AppRate(string data)
    {
        StoreHelper.AppRate();
    }

    private static void AppRecommend(string data)
    {
        StoreHelper.AppRecommend();
    }

    private static void ScreenPortrait(string data)
    {
        OrientationController.UsePortrait();
    }

    private static void ScreenLandscape(string data)
    {
        OrientationController.UseLandscape();
    }

    #endregion

    #region debughelper

    [ContextMenu("LaunchArScene AR_I01_Vaduz_Schloss")]
    private void LaunchArSceneVaduz()
    {
        LaunchArScene("AR_I01_Vaduz_Schloss");
        LaunchArScene("AR_I01_Vaduz_Schloss");
        LaunchArScene("AR_I01_Vaduz_Schloss");
    }

    [ContextMenu("Shutdown AR_I01_Vaduz_Schloss")]
    private void ShutdownArSceneVaduz()
    {
        ShutdownArScene();
        ShutdownArScene();
        ShutdownArScene();
    }

    [ContextMenu("MultiLaunch")]
    private void MultiLaunchTest()
    {
        LaunchArScene("AR_I01_Vaduz_Schloss");
        LaunchArScene("AR_I01_Vaduz_Schloss");
        ShutdownArScene();
        LaunchArScene("AR_I01_Vaduz_Schloss");
        ShutdownArScene();
        ShutdownArScene();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log($"OnApplicationPause = {pauseStatus}: {Application.targetFrameRate} FPS (vSyncCount: {QualitySettings.vSyncCount}");
    }

    #endregion
}

using AugmentedReality;
using AugmentedReality.Items;
using AugmentedReality.Poi;
using CameraHelper;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

// ReSharper disable RedundantDefaultMemberInitializer

public class ArDirector : MonoBehaviour
{
    private const int STATE_UNSUPPORTED = -1;
    private const int STATE_CHECKING_AVAILABILITY = 0;

    private const int STATE_SCANNING = 2;
    private const int STATE_PLACING = 3;
    private const int STATE_PLACING_POSSIBLE = 4;
    private const int STATE_SHOWING = 5;
    private const int STATE_PANORAMA = 6;

    private ScanningPanel _scanningPanel = null;
    private PlacingPanel _placingPanel = null;
    private ShowingPanel _showingPanel = null;
    private ARItem _arItem = null;

    private GenericStateMachine _stateMachine;
    private IARSessionController _arSessionController;
    private IARPlaneHit _lastArPlaneHit;
    private ClipPlaneOptimizer _clipPlaneOptimizer;

    private void Awake()
    {
        // get panels an references
        _scanningPanel = FindObjectOfType<ScanningPanel>();
        _placingPanel = FindObjectOfType<PlacingPanel>();
        _showingPanel = FindObjectOfType<ShowingPanel>();
        _arItem = FindObjectOfType<ARItem>();

        // create state machine and states
        _stateMachine = new GenericStateMachine(OnStateMachineInit);

        var unsupportedState = _stateMachine.AddState(STATE_UNSUPPORTED, "STATE_UNSUPPORTED");
        var checkingAvailabilityState = _stateMachine.AddState(
            STATE_CHECKING_AVAILABILITY,
            "STATE_CHECKING_AVAILABILITY"
        );
        var scanningState = _stateMachine.AddState(STATE_SCANNING, "STATE_SCANNING");
        var placingState = _stateMachine.AddState(STATE_PLACING, "STATE_PLACING");
        var placingPossibleState = _stateMachine.AddState(STATE_PLACING_POSSIBLE, "STATE_PLACING_POSSIBLE");
        var showingState = _stateMachine.AddState(STATE_SHOWING, "STATE_SHOWING");

        // define possible transitions
        var emptyTransition = new EmptyTransition();

        checkingAvailabilityState.AddTransition(unsupportedState, emptyTransition);
        checkingAvailabilityState.AddTransition(scanningState, emptyTransition);
        scanningState.AddTransition(placingState, emptyTransition);
        placingState.AddTransition(placingPossibleState, emptyTransition);
        placingPossibleState.AddTransition(placingState, emptyTransition);
        placingPossibleState.AddTransition(showingState, emptyTransition);
        showingState.AddTransition(placingState, emptyTransition);

        // set state enter and exit actions
        scanningState.OnEnterAction = OnScanningEnter;
        scanningState.OnExitAction = OnScanningExit;

        placingState.OnEnterAction = OnPlacingEnter;
        placingState.OnExitAction = OnPlacingExit;

        placingPossibleState.OnEnterAction = OnPlacingPossibleEnter;
        placingPossibleState.OnExitAction = OnPlacingPossibleExit;

        showingState.OnEnterAction = OnShowingEnter;
        showingState.OnExitAction = OnShowingExit;
        
    }

    private void Start()
    {
        // try to find the foundation or debug session controller
        _arSessionController = (IARSessionController) FindObjectOfType<ARFoundationSessionController>() ??
            FindObjectOfType<ARDebugSessionController>();

        _clipPlaneOptimizer = FindObjectOfType<ClipPlaneOptimizer>();
        if (!ReferenceEquals(_clipPlaneOptimizer, null)) { _clipPlaneOptimizer.enabled = false; }

        // append events
        _scanningPanel.OnCloseClick.RemoveAllListeners();
        _scanningPanel.OnCloseClick.AddListener(OnCloseClicked);

        _placingPanel.OnPlaceClick.RemoveAllListeners();
        _placingPanel.OnPlaceClick.AddListener(OnPlaceClicked);
        _placingPanel.OnScaleClick.RemoveAllListeners();
        _placingPanel.OnScaleClick.AddListener(OnScaleClicked);
        _placingPanel.OnCloseClick.RemoveAllListeners();
        _placingPanel.OnCloseClick.AddListener(OnCloseClicked);
        
        _showingPanel.OnCloseClick.AddListener(OnCloseClicked);
        _showingPanel.OnResetClick.AddListener(OnResetClicked);
        

        // init state machine into checking availability
        _stateMachine.RequestStateChange(STATE_CHECKING_AVAILABILITY);
    }

    private void Update()
    {
        // close ar scene if back is pressed on android
        #if UNITY_ANDROID || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // try to exit panorama state first, if it doesn't work
            // exit ar by  sending a close ar scene as coroutine, so it gets executed one frame later ;)
            OnCloseClicked();
        }
        #endif

        if (_stateMachine.IsInTransition || _stateMachine.CurrentState == null) { return; }

        switch (_stateMachine.CurrentState.Id) {
            case STATE_CHECKING_AVAILABILITY:
                UpdateCheckAvailability();
                break;
            case STATE_SCANNING:
                UpdateScanning();
                ProcessTouchInput(_scanningPanel);
                break;
            case STATE_PLACING:
                UpdatePlacing();
                ProcessTouchInput(_placingPanel);
                break;
            case STATE_PLACING_POSSIBLE:
                UpdatePlacingPossible();
                ProcessTouchInput(_placingPanel);
                break;
            case STATE_SHOWING: {
                ProcessTouchInput(_showingPanel);
                break;
            }
        }
    }

    private void UpdateCheckAvailability()
    {
        var systemState = _arSessionController.GetARSystemState();
        Debug.Log($"ArDirector.UpdateCheckAvailability: {systemState}");
#if UNITY_EDITOR
        systemState = ARSessionState.Ready;
#endif

        switch (systemState) {
            case ARSessionState.Unsupported:
                _stateMachine.RequestStateChange(STATE_UNSUPPORTED);
                return;
            case ARSessionState.SessionTracking:
            case ARSessionState.SessionInitializing:
            case ARSessionState.Ready:
                _stateMachine.RequestStateChange(STATE_SCANNING);
                return;
        }
    }

    private void UpdateScanning()
    {
        if (_arSessionController.HasDetectedPlaneInsight()) { _stateMachine.RequestStateChange(STATE_PLACING); }
    }

    private void UpdatePlacing()
    {
        if (_arSessionController.TryGetArPlaneHit(out _lastArPlaneHit)) {
            _stateMachine.RequestStateChange(STATE_PLACING_POSSIBLE);
        }
    }

    private void UpdatePlacingPossible()
    {
        if (_arSessionController.TryGetArPlaneHit(out _lastArPlaneHit)) {
            // update ar item position
            _arSessionController.MoveTransformToPose(_arItem.transform, _lastArPlaneHit, true);
        }
        else { _stateMachine.RequestStateChange(STATE_PLACING); }
    }

    #region state switch handling

    private void OnStateMachineInit(GenericState fromState)
    {
        _arSessionController.HidePlanes();
        _arSessionController.HidePointCloud();
    }

    private void OnScanningEnter(GenericState fromState)
    {
        _arItem.gameObject.SetActive(false);
        _scanningPanel.Show();
        _scanningPanel.StartSessionCheck(_arSessionController);
    }

    private void OnScanningExit(GenericState toState)
    {
        _scanningPanel.Hide();
    }

    private void OnPlacingEnter(GenericState fromState)
    {
        switch (fromState.Id) {
            case STATE_SCANNING:
                _arSessionController.ShowPlanes();
                _placingPanel.Show();
                _arItem.gameObject.SetActive(true);
                break;
            case STATE_SHOWING:
                _arSessionController.ShowPlanes();
                _placingPanel.Show();
                _arItem.InitAndHide();
                break;
        }
    }

    private void OnPlacingExit(GenericState toState) { }

    private void OnPlacingPossibleEnter(GenericState fromState)
    {
        Debug.Log("=> OnpLacingPossibleEnter");
        
        _arItem.ShowIndicator();
        _placingPanel.ShowButtons();
    }

    private void OnPlacingPossibleExit(GenericState toState)
    {
        Debug.Log("=> OnPlacingPossibleExit");
        
        switch (toState.Id) {
            case STATE_PLACING:

                // go back to placing
                _arItem.HideIndicator();
                _placingPanel.HideButtons(false);
                break;
            case STATE_SHOWING:
                _placingPanel.Hide();
                break;
        }
    }

    private void OnShowingEnter(GenericState fromState)
    {
        _arSessionController.HidePlanes();
        _showingPanel.Show();

        if (!ReferenceEquals(_clipPlaneOptimizer, null)) { _clipPlaneOptimizer.enabled = true; }

        _arSessionController.AttachToReferencePoint(_lastArPlaneHit, _arItem.transform);
        _arItem.ShowFullObject();
    }

    private void OnShowingExit(GenericState toState)
    {
        _arSessionController.DetachFromReferencePoint();

        if (!ReferenceEquals(_clipPlaneOptimizer, null)) { _clipPlaneOptimizer.enabled = false; }

        _showingPanel.Hide();
        ShowingPanelInteractionPoint interactionPointPanel = _showingPanel.GetComponent<ShowingPanelInteractionPoint>();
    }

    #endregion

    #region click event handlers

    private void OnScaleClicked()
    {
        if (!_stateMachine.IsInTransition && _stateMachine.CurrentState.Id == STATE_PLACING_POSSIBLE) {
            _arItem.NextScale();
        }
    }

    private void OnPlaceClicked()
    {
        if (!_stateMachine.IsInTransition && _stateMachine.CurrentState.Id == STATE_PLACING_POSSIBLE) {
            _stateMachine.RequestStateChange(STATE_SHOWING);
        }
    }

    private void OnResetClicked()
    {
        if (!_stateMachine.IsInTransition && _stateMachine.CurrentState.Id == STATE_SHOWING) {
            _stateMachine.RequestStateChange(STATE_PLACING);
        }
    }

    private void OnCloseClicked()
    {
        //SceneManager.UnloadSceneAsync("ARScene").completed += FindObjectOfType<MenuController>().OnARSceneClosed;
        
        _scanningPanel.transform.parent.gameObject.SetActive(false);
        
        //var mainController = FindObjectOfType<MainController>();
        //if (mainController != null) { mainController.ShutdownArScene(); }
    }
    
    #endregion

    private void ProcessTouchInput(GenericPanel actualPanel)
    {
        // editor input
        if (!_stateMachine.IsInTransition &&
            Application.isEditor &&
            Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject()) {
            actualPanel.TouchOnBlankScreen(Input.mousePosition);
            return;
        }

        // touch screen input
        if (!_stateMachine.IsInTransition && Input.touchCount == 1) {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                actualPanel.TouchOnBlankScreen(touch.position);
            }
        }
    }
}

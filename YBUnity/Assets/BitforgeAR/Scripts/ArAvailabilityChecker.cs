using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArAvailabilityChecker : MonoBehaviour
{
    public bool CheckFinished { get; private set; }
    public bool IsAvailable { get; private set; }

    private ARSession _arSession;

    private void Awake()
    {
        _arSession = GetComponent<ARSession>();
    }

    public void StartCheck()
    {
        // connect check state changes before enabling the ar session
        ARSession.stateChanged += OnArSessionStateChanged;

    if (_arSession != null) {
        // Activate ArSession
        _arSession.enabled = true;
    }
    }

    private void OnArSessionStateChanged(ARSessionStateChangedEventArgs e)
    {
        Debug.Log("AR-State: " + e.state);
        
        switch (e.state) {
            case ARSessionState.None:
            case ARSessionState.CheckingAvailability:

                // Not determined yet
                break;
            case ARSessionState.NeedsInstall:
            case ARSessionState.Installing:
            case ARSessionState.Ready:
            case ARSessionState.SessionInitializing:
            case ARSessionState.SessionTracking:
                OnAvailabilityDetermined(true);
                break;
            case ARSessionState.Unsupported:
                OnAvailabilityDetermined(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnAvailabilityDetermined(bool isAvailable)
    {
        CheckFinished = true;
        IsAvailable = isAvailable;
        
        ARSession.stateChanged -= OnArSessionStateChanged;
        //_arSession.enabled = false;
        //Destroy(_arSession);
    }
}

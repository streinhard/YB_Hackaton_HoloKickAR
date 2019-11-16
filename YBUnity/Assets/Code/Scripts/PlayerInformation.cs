using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInformation : MonoBehaviour
{
    public string PlayerName;
    public Team team;

    private NetworkState _networkState;
    
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (_networkState == null) { _networkState = FindObjectOfType < NetworkState>(); }

        if (_networkState != null) {
            if (_networkState.isServer) {
                _networkState.serverTeam = team;
            } else {
                _networkState.clientTeam = team;
            }
        }
    }
}

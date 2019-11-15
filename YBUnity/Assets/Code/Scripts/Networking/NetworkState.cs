using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkState : NetworkBehaviour
{
    [SerializeField]
    private FootballManager _footballManager;

    private int numberOfRegisteredPlayers;
    
    [Server]
    public void RegisterPlayer(PlayerIdentity identity)
    {
        numberOfRegisteredPlayers++;

        if (numberOfRegisteredPlayers == 2) { _footballManager.SpawnFootball(Vector3.up * 5f); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FootballManager : NetworkBehaviour
{
    public NetworkIdentity FootballPrefab;

    private NetworkIdentity _football;

    
    public void SpawnFootball(Vector3 position)
    {
        _football = Instantiate(FootballPrefab);
        
        NetworkServer.Spawn(_football.gameObject);
    }
}

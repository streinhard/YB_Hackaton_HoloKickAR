using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkState : NetworkBehaviour
{
    public NetworkField FootballNetworkFieldPrefab;

    public NetworkField PlayerPugFieldPrefab;

    private int numberOfRegisteredPlayers;

    private int serverScore;
    private int clientScore;
    
    private NetworkField _footballNetworkField;

    [Server]
    public void RegisterPlayer(PlayerIdentity identity)
    {
        numberOfRegisteredPlayers++;

        if (numberOfRegisteredPlayers == 2) {
            _footballNetworkField = Instantiate(FootballNetworkFieldPrefab);
            NetworkServer.Spawn(_footballNetworkField.gameObject);
        }

        GameObject newPlayerPug = Instantiate(PlayerPugFieldPrefab).gameObject;
        NetworkServer.SpawnWithClientAuthority(newPlayerPug, identity.gameObject);
    }
}

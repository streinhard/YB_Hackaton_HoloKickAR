using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkState : NetworkBehaviour
{
    public NetworkField FootballNetworkFieldPrefab;

    public NetworkField PlayerPugFieldPrefab;

    private int numberOfRegisteredPlayers;

    [SyncVar(hook = "OnChangeServerScore")]public int serverScore;
    [SyncVar(hook = "OnChangeClientScore")]public int clientScore;
    
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

    [Server]
    public void RegisterGoal(bool forServer)
    {
        Debug.Log("goal for " + forServer);
        
        if (forServer) {
            serverScore++;
        } else { clientScore++; }

        ballTransform = GameObject.FindWithTag("ball").transform;
        ballTransform.position = ballTransform.parent.position + Vector3.up * 0.1f;
        ballTransform.gameObject.SetActive(false);

        
        Invoke("SpawnBall", 2);
    }

    private Transform ballTransform;
    
    private void SpawnBall()
    {
        ballTransform.gameObject.SetActive(true);
    }

    private void OnChangeServerScore(int newServerScore)
    {
        if (isServer) {
            FindObjectOfType<ScoreText>().UpdateScore(newServerScore, clientScore);
        } else {
            FindObjectOfType<ScoreText>().UpdateScore(clientScore, newServerScore);
        }
    }
    
    private void OnChangeClientScore(int newClientScore)
    {
        if (isServer) {
            FindObjectOfType<ScoreText>().UpdateScore(serverScore, newClientScore);
        } else {
            FindObjectOfType<ScoreText>().UpdateScore(newClientScore, serverScore);
        }
    }
}

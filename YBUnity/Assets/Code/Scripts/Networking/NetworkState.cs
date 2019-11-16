using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkState : NetworkBehaviour
{
    public NetworkField FootballNetworkFieldPrefab;

    public NetworkField PlayerPugFieldPrefab;

    private int numberOfRegisteredPlayers;

    [SyncVar(hook = "OnChangeServerScore")]public int serverScore;
    [SyncVar(hook = "OnChangeClientScore")]public int clientScore;
    
    [SyncVar(hook = "OnChangeServerTeam")]public Team serverTeam;
    [SyncVar(hook = "OnChangeClientTeam")]public Team clientTeam;
    
    /*
    [SyncVar(hook = "OnChangeServerScore")]public int serverScore;
    [SyncVar(hook = "OnChangeClientScore")]public int clientScore;
    
    [SyncVar(hook = "OnChangeServerTeam")]public Team serverTeam;
    [SyncVar(hook = "OnChangeClientTeam")]public Team clientTeam;
    */

    private void OnChangeServerTeam(Team newServerTeam)
    {
        if (isServer) {
            GameObject.Find("TeamImagesL").GetComponent<TeamLogos>().SetTeam(newServerTeam);
        } else {
            GameObject.Find("TeamImagesR").GetComponent<TeamLogos>().SetTeam(newServerTeam);
        }

    }
    
    private void OnChangeClientTeam(Team newClientTeam)
    {
        if (isServer) {
            GameObject.Find("TeamImagesR").GetComponent<TeamLogos>().SetTeam(newClientTeam);
        } else {
            GameObject.Find("TeamImagesL").GetComponent<TeamLogos>().SetTeam(newClientTeam);
        }
    }
    
    
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

    public void SetClientTeam(Team team)
    {
        this.clientTeam = team;
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

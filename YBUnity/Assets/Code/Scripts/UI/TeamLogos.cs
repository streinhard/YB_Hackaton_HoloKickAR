using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TeamLogos : MonoBehaviour
{

    public bool isLeft;
    
    public GameObject[] teamImgs;

    private NetworkState _networkState;

    void Awake()
    {
        SetTeam(Team.NOTHING);
    }
    
    public void SetTeam(Team team)
    {
        foreach (GameObject teamImg in teamImgs) {
            teamImg.SetActive(false);
        }
        teamImgs[(int) team].SetActive(true);
    }

    void Update()
    {
        if (_networkState == null) { _networkState = FindObjectOfType < NetworkState>(); }

        if (_networkState != null) {
            if (_networkState.isServer) {
                SetTeam(isLeft ? _networkState.serverTeam : _networkState.clientTeam);
            } else {
                SetTeam(isLeft ? _networkState.clientTeam : _networkState.serverTeam);
            }
        }
    }
}

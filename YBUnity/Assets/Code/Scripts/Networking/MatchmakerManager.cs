using System.Collections;
using System.Collections.Generic;
using AugmentedReality.Items;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchmakerManager : MonoBehaviour
{
    [SerializeField]
    private ArDirector arDirector;

    private NetworkManager networkManager;
    
    private bool _hasDoneMatchListRequest;

    private bool startedNetworking;
    
    public void StartNetworking()
    {
        startedNetworking = true;
    }

    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startedNetworking) return;
        
        if (networkManager.matchMaker == null) { // didnt start matchmaker yet
            Debug.Log("startMatchmaker");
            networkManager.StartMatchMaker();
        } else {
            if (networkManager.matchInfo == null) { // started matchmaker but didn't join match yet
                if (!_hasDoneMatchListRequest) {
                    // no matches are listed
                    Debug.Log("Try List Matches");
                    networkManager.matchMaker.ListMatches( // try get the lists
                        0,
                        20,
                        "",
                        false,
                        0,
                        0,
                        this.OnMatchList
                    );
                    _hasDoneMatchListRequest = true;
                }
            }
        }
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        networkManager.OnMatchList(success, extendedInfo, matchList); // let the netw manager know, for the UI

        if (matchList.Count == 0) { // no matches, create one
            Debug.Log("create Match");
            networkManager.matchMaker.CreateMatch(
                networkManager.matchName,
                networkManager.matchSize,
                true,
                "",
                "",
                "",
                0,
                0,
                networkManager.OnMatchCreate
            );
        } else { // got at least one match
            for (int i = 0; i < networkManager.matches.Count; i++) {
                var match = matchList[0];
                networkManager.matchName = match.name;
                networkManager.matchMaker.JoinMatch(
                    match.networkId,
                    "",
                    "",
                    "",
                    0,
                    0,
                    networkManager.OnMatchJoined
                );
            }
        }
    }
    
    void OnApplicationQuit()
    {
        Debug.Log("Closing Match");
        networkManager.StopHost();
    }
    
}

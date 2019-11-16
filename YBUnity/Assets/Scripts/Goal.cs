using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Goal : MonoBehaviour
{
    public GoalType goalType;

    public bool belongsToServer;

    public GameObject soccerField;

    // Start is called before the first frame update
    void Start()
    {
        soccerField = GameObject.FindWithTag("soccerField");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.CompareTag("ball")) {
            bool isServer = NetworkManager.singleton.client.connection.playerControllers[0].gameObject
                .GetComponent<NetworkBehaviour>()
                .isServer;


            if (!isServer) return;
            
            

            if (goalType == GoalType.Opponent)
            {
                Debug.Log("GOAL AYYYY");
                FindObjectOfType<NetworkState>().RegisterGoal(true);
                
                soccerField.GetComponent<SoccerField>().scoreHome += 1;
                soccerField.GetComponent<SoccerField>().showHomeParticles();
                Invoke(nameof(stopParticles), 5);
            }
            else if (goalType == GoalType.Home)
            {
                Debug.Log("Neeeein, eigentor!");
                
                FindObjectOfType<NetworkState>().RegisterGoal(false);
            }
        }
    }

    private void stopParticles()
    {
        soccerField.GetComponent<SoccerField>().hideHomeParticles();
    }
}

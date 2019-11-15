using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GoalType GoalType;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ball"))
        {
            if (GoalType == GoalType.Opponent)
            {
                Debug.Log("GOAL AYYYY");
                GameObject homeScoreBillboard = GameObject.Find("HomeScore");
                
                int homeScore = Int32.Parse(homeScoreBillboard.GetComponent<TextMeshProUGUI>().text);
                homeScore++;
                homeScoreBillboard.GetComponent<TextMeshProUGUI>().text = homeScore.ToString();
            }
            else if (GoalType == GoalType.Home)
            {
                Debug.Log("Neeeein, eigentor!");
            }
        }
    }
}

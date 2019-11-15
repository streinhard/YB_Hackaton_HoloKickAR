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
            }
            else if (GoalType == GoalType.Home)
            {
                Debug.Log("Neeeein, eigentor!");
            }
        }
    }
}

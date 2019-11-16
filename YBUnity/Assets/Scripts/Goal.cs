using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GoalType goalType;

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
        if (other.gameObject.CompareTag("ball"))
        {
            if (goalType == GoalType.Opponent)
            {
                Debug.Log("GOAL AYYYY");
                soccerField.GetComponent<SoccerField>().scoreHome += 1;
                soccerField.GetComponent<SoccerField>().showHomeParticles();
                Invoke(nameof(stopParticles), 5);
                //
            }
            else if (goalType == GoalType.Home)
            {
                Debug.Log("Neeeein, eigentor!");
            }
        }
    }

    private void stopParticles()
    {
        soccerField.GetComponent<SoccerField>().hideHomeParticles();
    }
}

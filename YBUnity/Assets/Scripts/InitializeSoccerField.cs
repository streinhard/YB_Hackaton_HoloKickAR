﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSoccerField : MonoBehaviour
{
    private GameObject particlesHome;
    private GameObject particlesOpponent;

    public int scoreHome;
    public int scoreOpponent;

    // Start is called before the first frame update
    void Start()
    {
        particlesHome = GameObject.FindWithTag("particlesHome");
        particlesOpponent = GameObject.FindWithTag("particlesOpponent");
        
        particlesHome.SetActive(false);
        particlesOpponent.SetActive(false);
    }

    public void showHomeParticles()
    {
        Debug.Log("Show Home Particles");
        Debug.Log(scoreHome);
        particlesHome.SetActive(true);
        particlesOpponent.SetActive(true);
    }
    
    public void hideHomeParticles()
    {
        Debug.Log("Hide Home Particles");
        particlesHome.SetActive(false);
        particlesOpponent.SetActive(false);
    }
}

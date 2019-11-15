using System.Collections;
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
        particlesHome.SetActive(true);
        particlesOpponent.SetActive(true);
        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
    }
    
    public void hideHomeParticles()
    {
        particlesHome.SetActive(false);
        particlesOpponent.SetActive(false);
    }
}

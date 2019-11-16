using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public string PlayerName;
    public Team team;

    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}

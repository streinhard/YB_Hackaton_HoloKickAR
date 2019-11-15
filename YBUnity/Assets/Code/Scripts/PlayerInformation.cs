using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public string PlayerName;

    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}

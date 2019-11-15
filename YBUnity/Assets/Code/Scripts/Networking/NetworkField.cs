using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkField : NetworkBehaviour
{
    private float timeSinceCreation;

    // Start is called before the first frame update
    void Awake()
    {
        Transform itemAnchorTrans = GameObject.FindWithTag("ItemAnchor").transform;
        
        transform.SetParent(itemAnchorTrans, true);
        transform.position = itemAnchorTrans.position;
        transform.rotation = itemAnchorTrans.rotation;
    }

    void Update()
    {
        if (timeSinceCreation < 1f) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            timeSinceCreation += Time.deltaTime;
        }

    }
}

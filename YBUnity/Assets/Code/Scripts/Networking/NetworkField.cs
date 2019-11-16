using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkField : NetworkBehaviour
{
    private float timeSinceCreation;

    private Quaternion setRotation;

    private bool calledStart;

    // Start is called before the first frame update
    public void Start()
    {
        if (calledStart) return;
        
        Transform itemAnchorTrans = GameObject.FindWithTag("ItemAnchor").transform;
        
        transform.SetParent(itemAnchorTrans, true);
        transform.position = itemAnchorTrans.position;
        transform.rotation = itemAnchorTrans.rotation;
        setRotation = transform.localRotation;

        var playerController = GetComponentInChildren<PlayerController>();
        if (playerController != null) {
            playerController.invertedSides = true;
            
            transform.rotation = transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
            setRotation = transform.localRotation;
        }

        Transform firstChild = transform.GetChild(0);
        if (firstChild.gameObject.CompareTag("ball") && isServer) {
            transform.rotation = transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
            setRotation = transform.localRotation;
        }
        
        calledStart = true;
    }

    
    
    public override void OnStartAuthority()
    {
        if (!calledStart) {
            Start();
        }
        
        var playerController = GetComponentInChildren<PlayerController>();
        if (playerController != null) {
            playerController.invertedSides = false;
            playerController.UpdatePlayerArea();
            
            transform.rotation = transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
            
            setRotation = transform.localRotation;
        }
        
        
        base.OnStartAuthority();
    }

    void Update()
    {
        if (timeSinceCreation < 1f) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = setRotation;
            
            timeSinceCreation += Time.deltaTime;
        }
    }
}

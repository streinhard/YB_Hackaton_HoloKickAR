using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardBehaviour : MonoBehaviour
{
    private float mainTextureOffsetY;
    private float mainTextureOffsetX;
    
    private Vector2 uvAnimationRate = new Vector2( 0.0f, .5f );

    private bool run;
    private int i;

    Vector2 uvOffset = Vector2.zero;
    void LateUpdate() 
    {
        if (run)
        {
            uvOffset += uvAnimationRate * Time.deltaTime;
            if(gameObject.GetComponent<MeshRenderer>().enabled)
            {
                gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset = uvOffset;
            }   
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mainTextureOffsetY = gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset.y;
        mainTextureOffsetX = gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset.x;
        run = false;
        
        InvokeRepeating(nameof(ChangeBillboard), 5f, 1f);
    }

    private void ChangeBillboard()
    {
        if (i % 10 == 0)
        {
            run = true;
        }
        else
        {
            run = false;
        }

        i++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPos;
    public bool isMoving;
    const int MOUSE = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        isMoving = false;
        
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
            Code for locking cursor
         
            if (Input.GetKeyDown("1"))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
            if (Input.GetKeyDown("2"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        */
        
      
        // Code to set position to mouse position
        /*
        SetTarggetPosition();
        
        if(isMoving)
        {
            MoveObject();
        }
        */
        
        
        
    }
    
    void SetTarggetPosition()
    {
        Plane plane = new Plane(Vector3.up,transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;
 
        if(plane.Raycast(ray, out point))
            targetPos = ray.GetPoint(point);
         
        isMoving = true;
    }
    void MoveObject()
    {
        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
 
        if (transform.position == targetPos)
            isMoving = false;
        Debug.DrawLine(transform.position,targetPos,Color.red);
 
    }
}

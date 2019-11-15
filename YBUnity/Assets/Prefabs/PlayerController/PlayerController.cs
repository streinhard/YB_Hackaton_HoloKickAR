using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public float speed = 10f;
    public Vector3 targetPos;
    public bool isMoving;
    const int MOUSE = 0;
    
    // Lerpyderpy stuff
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;
    
    // Total distance between the markers.
    private float journeyLength;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        isMoving = false;
        
        Cursor.visible = false;
        
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        
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

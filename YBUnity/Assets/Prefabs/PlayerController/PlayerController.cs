using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public float speed = 10f;
    public Vector3 targetPos;
    public bool isMoving;
    const int MOUSE = 0;

    public float speed;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        isMoving = false;

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
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

        // Code to set position to mouse position
        SetTargetPosition();

        if (isMoving)
        {
            MoveObject();
        }
    }

    void SetTargetPosition()
    {
        var plane = new Plane(Vector3.up,transform.position);
        var ray = camera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out var point)) {
            targetPos = ray.GetPoint(point);
        }

        isMoving = true;
    }

    void MoveObject()
    {
        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (transform.position == targetPos) {
            isMoving = false;
        }
    }
}

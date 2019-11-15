using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public float speed = 10f;
    public Vector3 targetPos;
    public float speed;

    private bool cursorLocked;

    public Transform soccerField;
    public Transform flagBottomLeft;
    public Transform flagTopRight;

    private Vector3 areaBottomLeft;
    private Vector3 areaTopRight;
    private Vector3 areaSize;
    private float height;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        camera = Camera.main;

        SetPlayerArea();
    }

    // Update is called once per frame
    private void Update()
    {
        CursorLockUpdate();

        if (!cursorLocked) return;

        SetTargetPosition();
        MoveObject();
    }

    private void SetPlayerArea()
    {
        areaBottomLeft = flagBottomLeft.position;
        areaTopRight = flagTopRight.position;
        areaSize = new Vector3(areaTopRight.x - areaBottomLeft.x, 0, areaTopRight.z - areaBottomLeft.z);
        height = soccerField.position.y + 0.01f;
    }

    private void CursorLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) { cursorLocked = false; }
        else if (Input.GetMouseButtonUp(0)) { cursorLocked = true; }

        if (cursorLocked) {
            Cursor.visible = false;
        } else if (!cursorLocked) {
            Cursor.visible = true;
        }
    }

    private void SetTargetPosition()
    {
        var pos = Input.mousePosition;
        var viewPortPos = camera.ScreenToViewportPoint(pos);

        var xPos = Mathf.Clamp(viewPortPos.x, 0, 1);
        var yPos = Mathf.Clamp(viewPortPos.y, 0, 1);

        var x = areaBottomLeft.x + areaSize.x * xPos;
        var z = areaBottomLeft.z + areaSize.z * yPos;
        targetPos = new Vector3(x, height, z);
    }

    private void MoveObject()
    {
        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}

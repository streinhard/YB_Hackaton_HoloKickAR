using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform soccerField;
    public Transform flagBottomLeft;
    public Transform flagTopRight;

    public Transform targetPos;

    private bool cursorLocked;

    private Vector3 areaBottomLeft;
    private Vector3 areaTopRight;
    private Vector3 areaSize;
    private float height;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        SetPlayerArea();
    }

    // Update is called once per frame
    private void Update()
    {
        CursorLockUpdate();

        if (!cursorLocked) return;

        SetTargetPosition();
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
        if (Input.GetKeyUp(KeyCode.Escape)) {
            cursorLocked = false;
        } else if (Input.GetMouseButtonUp(0)) {
            cursorLocked = !cursorLocked;
        }

        Cursor.visible = !cursorLocked;
    }

    private void SetTargetPosition()
    {
        var pos = Input.mousePosition;
        var viewPortPos = camera.ScreenToViewportPoint(pos);

        var xPos = Mathf.Clamp(viewPortPos.x, 0, 1);
        var yPos = Mathf.Clamp(viewPortPos.y, 0, .25f) * 4f;

        var x = areaBottomLeft.x + areaSize.x * xPos;
        var z = areaBottomLeft.z + areaSize.z * yPos;
        targetPos.position = new Vector3(x, height, z);
    }
}

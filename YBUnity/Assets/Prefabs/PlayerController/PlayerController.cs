using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SoccerField soccerField;
    public Rigidbody puck;

    public float PuckSpeed = 5;

    private Vector3 targetPos;
    private Camera camera;
    private bool cursorLocked;

    private Vector3 areaBottomLeft;
    private Vector3 areaTopRight;
    private Vector3 areaSize;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        soccerField = FindObjectOfType<SoccerField>();
        camera = Camera.main;
        SetPlayerArea();
    }

    // Update is called once per frame
    private void Update()
    {
        CursorLockUpdate();

        if (!cursorLocked) return;

        UpdateTargetPosition();
        MovePuck();
    }

    private void SetPlayerArea()
    {
        areaBottomLeft = soccerField.flagBottomLeft.position;
        areaTopRight = soccerField.flagTopRight.position;
        areaSize = new Vector3(areaTopRight.x - areaBottomLeft.x, 0, areaTopRight.z - areaBottomLeft.z);
        height = soccerField.transform.position.y + 0.01f;
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

    private void UpdateTargetPosition()
    {
        var pos = Input.mousePosition;
        var viewPortPos = camera.ScreenToViewportPoint(pos);

        var xPos = Mathf.Clamp(viewPortPos.x, 0, 1);
        var yPos = Mathf.Clamp(viewPortPos.y, 0, .25f) * 4f;

        var x = areaBottomLeft.x + areaSize.x * xPos;
        var z = areaBottomLeft.z + areaSize.z * yPos;
        targetPos = new Vector3(x, height, z);
    }

    private void MovePuck()
    {
        var puckPos = puck.position;
        var distance = Vector3.Distance(targetPos, puckPos);
        var speed = distance > 0.01 ? PuckSpeed : 0;
        var direction = targetPos - puckPos;
        puck.velocity = speed * direction;

    }
}

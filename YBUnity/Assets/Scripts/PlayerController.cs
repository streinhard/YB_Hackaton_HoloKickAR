using System;
using System.Collections;
using System.Collections.Generic;
using AugmentedReality;
using UnityEngine;
using UnityEngine.Networking;

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

    private NetworkBehaviour _networkBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        soccerField = FindObjectOfType<SoccerField>();

        camera = Camera.main;
        FindArCamera();

        UpdatePlayerArea();

        if (transform.parent != null) {
            _networkBehaviour = transform.parent.GetComponent<NetworkBehaviour>();
        }
    }

    private void FindArCamera()
    {
        object foundationConteroller = FindObjectOfType<ARFoundationSessionController>();
        object debugController = FindObjectOfType<ARDebugSessionController>();
        var arSessionController = foundationConteroller as IARSessionController;
        arSessionController = arSessionController ?? debugController as IARSessionController;
        if (arSessionController != null) { camera = arSessionController.ARCamera; }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_networkBehaviour != null && !_networkBehaviour.hasAuthority) return;

        CursorLockUpdate();

        if (!cursorLocked) return;

        UpdateTargetPosition();
        MovePuck();
    }


    private void UpdatePlayerArea()
    {
        areaBottomLeft = soccerField.flagBottomLeft.position;
        areaTopRight = soccerField.flagTopRight.position;
        areaSize = areaTopRight - areaBottomLeft;
        areaSize.z = areaSize.z / 2;
        height = soccerField.transform.position.y;
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
        Vector2 pos;
        if (Input.touchCount > 0) {
            pos = Input.GetTouch(0).position;
        } else {
            pos = Input.mousePosition;
        }
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
        direction.y = 0;
        puck.velocity = speed * direction;

    }
}

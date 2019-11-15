using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
	public Camera Camera;
	public GameObject BallPrefab;
	public float BallSpeed = 300.0f;

	private readonly ColorGenerator _colorGenerator = new ColorGenerator();

	private void Update ()
	{
		if (ShouldThrowBall()) {

			var newBall = Instantiate(BallPrefab, Camera.transform.position, Quaternion.identity);
			newBall.transform.parent = transform;

			var mesh = newBall.GetComponent<MeshRenderer>();
			mesh.material.color = _colorGenerator.GetNext();

			var body = newBall.GetComponent<Rigidbody>();
			body.AddForce(Camera.transform.forward * BallSpeed);

			if (transform.childCount > 10) {
				var oldest = transform.GetChild(0);
				Destroy(oldest.gameObject);
			}
		}
	}

	private bool ShouldThrowBall()
	{
		var isMouseDown = Input.GetMouseButtonDown(0);
		var hasTouchBegan = Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began;

		if (!isMouseDown && !hasTouchBegan) return false;

		var touchPoint = Vector3.zero;
		if (isMouseDown) touchPoint = Camera.ScreenToViewportPoint(Input.mousePosition);
		if (hasTouchBegan) touchPoint = Camera.ScreenToViewportPoint(Input.touches[0].position);

		return  touchPoint.y > 0.1;
	}

	private void FixedUpdate()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit();
			}
		}
	}
}

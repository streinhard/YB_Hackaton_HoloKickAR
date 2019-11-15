using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SecretReset : MonoBehaviour
{
	private DateTime? _touchStartTime;

	private void Update ()
	{
		if (Input.touches.Length > 2) {
			GetComponent<ARSession>().Reset();
		}
	}
}

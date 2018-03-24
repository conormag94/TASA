using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class MapController : MonoBehaviour {

	private Quaternion targetRotation;
	private float rotateSpeed = 60.0f;
	private float rotateAmount = 30.0f;

	// Use this for initialization
	void Start () {
		targetRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
	}

	public void RotateClockwise () {
		targetRotation = Quaternion.AngleAxis (rotateAmount, Vector3.up) * targetRotation; 
	}

	public void RotateAntiClockwise () {
		targetRotation = Quaternion.AngleAxis (-rotateAmount, Vector3.up) * targetRotation;
	}
}

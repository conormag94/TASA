using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

	public LineRenderer lr;
	public Transform origin;
	public Transform destination;

	public Vector3 start;
	public Vector3 middle;
	public Vector3 end;


	private float distance;
	private float counter;


	private float distance2;
	private float counter2;

	public float renderSpeed = 6f;

	// Use this for initialization
	void Start () {

		start = new Vector3 (normalizex(53.343641f), 0f, normalizey(-6.25068f));
		middle = new Vector3 (normalizex(53.341921f), 0f, normalizey(-6.252864f));
		end = new Vector3 (normalizex(53.341959f), 0f, normalizey(-6.250519f));

		Debug.Log (start);
		Debug.Log (middle);
		Debug.Log (end);

		lr = GetComponent<LineRenderer> ();
		//		lr.SetPosition (0, origin.position);
		lr.SetPosition (0, start);
		lr.SetWidth (0.45f, 0.45f);

		//		distance = Vector3.Distance (origin.position, destination.position);
		distance = Vector3.Distance (start, middle);
		distance2 = Vector3.Distance (middle, end);
	}

	// Update is called once per frame
	void Update () {

		if (counter < 9) {
			counter += .1f / renderSpeed;

			float x = Mathf.Lerp (0, distance, counter);

			//			Vector3 pointA = origin.position;
			//			Vector3 pointB = destination.position;
			Vector3 pointA = start;
			Vector3 pointB = middle;

			Vector3 pointAlongLine = x * Vector3.Normalize (pointB - pointA) + pointA;

			lr.SetPosition (1, pointAlongLine);
			Debug.Log (counter);
			Debug.Log (distance);
			Debug.Log (pointAlongLine);
		} 
		else {
			if (counter2 < distance2) {
				counter2 += .1f / renderSpeed;

				float x = Mathf.Lerp (0, distance2, counter2);

				//			Vector3 pointA = origin.position;
				//			Vector3 pointB = destination.position;
				Vector3 pointA = middle;
				Vector3 pointB = end;

				Vector3 pointAlongLine = x * Vector3.Normalize (pointB - pointA) + pointA;

				lr.SetPosition (1, pointAlongLine);

				Debug.Log (pointAlongLine);

			} 

		}

	}

	public float normalizex(float value){
		float minX = 53.341921f;
		float maxX = 53.343641f;

		return normalize (minX, maxX, value);
	}

	public float normalizey(float value){
		float minY = -6.252864f;
		float maxY = -6.250519f;

		return normalize (minY, maxY, value);
	}

	public float normalize(float min, float max, float value){
		return (2 *(value - min) / (max - min) - 1) * 10;
	}

}

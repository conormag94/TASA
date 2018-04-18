using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class MapController : MonoBehaviour {

	// The mapbox AbstractMap component of the game object
	[SerializeField]
	private AbstractMap _map;

	private Quaternion targetRotation;
	private float rotateSpeed = 90.0f;
	private float rotateAmount = 30.0f;

	private List<Vector2d> mapCoordinates = new List<Vector2d> ();
	private List<int> mapZoomLevels = new List<int> ();
	private int currentMapIndex = 0;


	// Use this for initialization
	void Start () {
		targetRotation = transform.rotation;
		AddListOfMaps ();
		_map.Initialize (mapCoordinates [currentMapIndex], mapZoomLevels [currentMapIndex]);
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
	}

	private void AddListOfMaps () {
		// San Fran (initial map)
		mapCoordinates.Add(new Vector2d(53.3441655,-6.2532845));
		mapZoomLevels.Add (16);

		// The alps
		mapCoordinates.Add (new Vector2d (45.0079330, 6.1464520));
		mapZoomLevels.Add (13);

		// Yosemite
		mapCoordinates.Add(new Vector2d (37.7384597,-119.592332));
		mapZoomLevels.Add (12);

	}

	public void RotateClockwise () {
		targetRotation = Quaternion.AngleAxis (rotateAmount, Vector3.up) * targetRotation; 
	}

	public void RotateAntiClockwise () {
		targetRotation = Quaternion.AngleAxis (-rotateAmount, Vector3.up) * targetRotation;
	}

	public void NextMap () {
		currentMapIndex = (currentMapIndex + 1) % mapCoordinates.Count;
		_map.Initialize (mapCoordinates [currentMapIndex], mapZoomLevels [currentMapIndex]);
	}

	public void PreviousMap () {
		currentMapIndex = (currentMapIndex - 1) % mapCoordinates.Count;
		_map.Initialize (mapCoordinates [currentMapIndex], mapZoomLevels [currentMapIndex]);
	}
}

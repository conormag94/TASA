
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System;
using System.IO;

public class StravaSpheres : MonoBehaviour
{
	[SerializeField]
	AbstractMap _map;

	[SerializeField]
	Vector2d[] _locations;

	[SerializeField]
	float _spawnScale = 0.02f;

	[SerializeField]
	GameObject _markerPrefab;

	[SerializeField]
	private float rayOriginHeight = 0.01f;

	int numberStravaCoords = 134;

	List<GameObject> _spawnedObjects;

	private float counter;

	public float renderSpeed = .2f;

	private Boolean drawSpheres = false;

	public Vector3[] positions;

	void Start()
	{
		string locations_str = GetStravaCoords("darragh", "1437408506");
		var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>> (locations_str);
		Debug.Log (locations [0] [0]);

		_locations = new Vector2d[numberStravaCoords];
		_spawnedObjects = new List<GameObject>();
		positions = new Vector3[numberStravaCoords];

		for (int i = 0; i < numberStravaCoords; i++)
		{
			var locationString = "" + locations[i][0] + ", " + locations[i][1];
			_locations[i] = Conversions.StringToLatLon(locationString);
			Vector3 worldPosition = _map.GeoToWorldPosition(_locations[i]);

			if (drawSpheres == true) {
				var instance = Instantiate (_markerPrefab);
				instance.transform.localPosition = worldPosition;
				Debug.Log (instance.transform.localPosition);
				instance.transform.localScale = Vector3.one * _spawnScale;
				_spawnedObjects.Add (instance);
			} else {
				positions [i] = worldPosition;
			}
		}
	}
		
	private void Update()
	{

//		int count = _spawnedObjects.Count;

		if (counter < (float)numberStravaCoords) {
			counter += renderSpeed;
			Debug.Log (counter);
		}

		for (int i = 0; Convert.ToSingle(i) < counter; i++)
		{
			var location = _locations [i];
			if (drawSpheres) {
				var spawnedObject = _spawnedObjects [i];
				var pointPosition = _map.GeoToWorldPosition (location);
				pointPosition.y += _map.WorldRelativeScale;
				spawnedObject.transform.localPosition = pointPosition;
			} else {
				positions[i] = _map.GeoToWorldPosition (location);
				PerformSnap (positions[i]);
			}
//			PerformSnap (spawnedObject);
//			spawnedObject.transform.localPosition = new Vector3(10.0f, 10.0f, 10.0f);
//				spawnedObject.transform.localScale = Vector3.one * _spawnScale;
		}

		for (int i = (int)counter; i < numberStravaCoords; i++)
		{
			if(drawSpheres){
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = new Vector3(10.0f, 10.0f, 10.0f);
//				spawnedObject.transform.localScale = Vector3.one * _spawnScale;		
			}
		}

	}

	public void PerformSnap(Vector3 routePostion)
	{
		Vector3 rayOrigin = new Vector3 (
			routePostion.x, 
			rayOriginHeight, 
			routePostion.z);

		Vector3 rayDirection = Vector3.down * rayOriginHeight * 2;
		Ray ray = new Ray (rayOrigin, rayDirection);

		RaycastHit[] hitPointList = Physics.RaycastAll(ray);
//			Debug.DrawRay(ray.origin, ray.direction * rayOriginHeight * 2, Color.red);

		if (hitPointList.Length > 0) {
			// Get the raycast hit point
			Vector3 hitPoint = hitPointList [0].point + new Vector3 (0, 0, 0);

			Vector3 newPos = new Vector3 (
				routePostion.x, 
				hitPoint.y, 
				routePostion.z);

			// Apply elevation
//			spawnedObject.transform.position = newPos;

//				Debug.DrawLine(newPos, new Vector3(newPos.x, newPos.y + 0.05f, newPos.z), Color.cyan);
			DrawLine(new Vector3(newPos.x, newPos.y + 0.03f, newPos.z), newPos, new Color(0f, 1f, 1f, 0.08f));
		}

	}

	private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
	{
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

		lr.SetColors(color, color);

		lr.SetWidth(0.005f, 0.005f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}

	public void swapRoute(string user, string activity_id, int stravaCoords){
		counter = 0;
		numberStravaCoords = stravaCoords;

		string locations_str = GetStravaCoords(user, activity_id);
		var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>> (locations_str);
		Debug.Log ("Route Swap");

		_locations = new Vector2d[numberStravaCoords];
		_spawnedObjects = new List<GameObject>();
		positions = new Vector3[numberStravaCoords];

		for (int i = 0; i < numberStravaCoords; i++)
		{
			var locationString = "" + locations[i][0] + ", " + locations[i][1];
			_locations[i] = Conversions.StringToLatLon(locationString);
			Vector3 worldPosition = _map.GeoToWorldPosition(_locations[i]);

			if (drawSpheres == true) {
				var instance = Instantiate (_markerPrefab);
				instance.transform.localPosition = worldPosition;
				Debug.Log (instance.transform.localPosition);
				instance.transform.localScale = Vector3.one * _spawnScale;
				_spawnedObjects.Add (instance);
			} else {
				positions [i] = worldPosition;
			}
		}

	}

	public string GetStravaCoords(string user, string activity_id)
	{
		string url = "http://127.0.0.1:8010/activity?user=" + user + "&id=" + activity_id;
//		string url = "http://b77c79a7.ngrok.io/activity?user=" + user + "&id=" + activity_id;
		string activity_stream = Get (url);
		Debug.Log (activity_stream);
		return activity_stream;
	}

	public string Get(string uri)
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
		request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

		using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
		using(Stream stream = response.GetResponseStream())
		using(StreamReader reader = new StreamReader(stream))
		{
			return reader.ReadToEnd();
		}
	}
}
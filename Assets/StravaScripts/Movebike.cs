﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Net;
using System;
using System.IO;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;

public class Movebike : MonoBehaviour {

	public Transform[] target;
	public float speed;

	private int current;

	private Vector3[] stravaTarget;

	int numberOfStravaCoords = 134;

	[SerializeField]
	Vector2d[] _locations;

	[SerializeField]
	AbstractMap _map;

//	void Start(){
//		current = 0;
//		string loc_str = GetStravaCoords();
//		var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>> (loc_str);
//		stravaTarget = new Vector3[numberOfStravaCoords];
//		_locations = new Vector2d[numberOfStravaCoords];
//
//		Debug.Log ("into for");
//		for (int i = 0; i < numberOfStravaCoords; i++) { // _locationStrings.Length
//			var locationString = "" + locations [i] [0] + ", " + locations [i] [1]; // "53.347440, -6.254188"
//			_locations [i] = Conversions.StringToLatLon (locationString);
//			Debug.Log (_locations [i]);
//			stravaTarget [i] = _map.GeoToWorldPosition (_locations [i]);
//			Debug.Log (i);
//			Debug.Log (stravaTarget [i].x);
//			Debug.Log (stravaTarget [i].z);
//
//		}
//	}

	void Start()
	{
		current = 0;
		string locations_str = GetStravaCoords();
		var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>> (locations_str);
		stravaTarget = new Vector3[numberOfStravaCoords];
		_locations = new Vector2d[numberOfStravaCoords];

		Debug.Log ("here");
		Debug.Log (_map.name);

		for (int i = 0; i < numberOfStravaCoords; i++)
		{
			var locationString = "" + locations[i][0] + ", " + locations[i][1];
			_locations[i] = Conversions.StringToLatLon(locationString);
			Vector3 instance = _map.GeoToWorldPosition(_locations[i]);
			stravaTarget [i] = instance;	

			//				Debug.Log("start local scale");
			//				Debug.Log (instance.transform.localScale.x);
			//				Debug.Log (instance.transform.localScale.y);
			//				Debug.Log (instance.transform.localScale.z);

		}
	}

	void Update (){
		Debug.Log (transform.position.x);
		Debug.Log (transform.position.y);
		Debug.Log (transform.position.z);
		Debug.Log (current);
		Debug.Log (stravaTarget[current].x);
		Debug.Log (stravaTarget[current].y);
		Debug.Log (stravaTarget[current].z);
		if (transform.position != stravaTarget[current]) {
			Vector3 pos = Vector3.MoveTowards (transform.position, stravaTarget[current], speed * Time.deltaTime);
			GetComponent<Rigidbody> ().MovePosition (pos);
			Debug.Log ("moving");
			Debug.Log (pos);
		} else {
			current = (current + 1) % stravaTarget.Length;
		}

	}

	public string GetStravaCoords()
	{
		string url = "http://127.0.0.1:8010/stream";
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
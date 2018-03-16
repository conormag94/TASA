using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;

public class StravaClient : MonoBehaviour {

	public float longitude;
	public float latitude;
	public Rigidbody rb;
	[SerializeField]
	public AbstractMap _map;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Debug.Log("hello world");
		Console.WriteLine("Hello World!");
		string url = "http://127.0.0.1:8010/stream";
		string activity_stream = Get(url);
		Debug.Log (activity_stream);
		//		LatLng latlng = LatLng.CreateFromJSON (activity_stream);
		var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>>(activity_stream);
		Debug.Log (locations[0][0]);
		Mapbox.Utils.Vector2d location = new Mapbox.Utils.Vector2d (53.4f, -6.0f);
		Vector3 new_location = _map.GeoToWorldPosition (location);
		Debug.Log (new_location);

	}

	// Update is called once per frame
	void Update () {
		//		Vector3 movement = new Vector3(0.01f, 0.0f, 0.01f);
		//		rb.AddForce(movement);
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

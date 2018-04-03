namespace Mapbox.Examples
{
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

		int numberStravaCoords = 134;

		List<GameObject> _spawnedObjects;

		private float counter;

		public float renderSpeed = 100f;

		void Start()
		{
			string locations_str = GetStravaCoords();
			var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>> (locations_str);
			Debug.Log (locations [0] [0]);

			_locations = new Vector2d[numberStravaCoords];
			_spawnedObjects = new List<GameObject>();

			for (int i = 0; i < numberStravaCoords; i++)
			{
				var locationString = "" + locations[i][0] + ", " + locations[i][1];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i]);
				Debug.Log (instance.transform.localPosition);
				instance.transform.localScale = Vector3.one * _spawnScale;
				_spawnedObjects.Add(instance);
			}
		}
			
		private void Update()
		{
			Vector3[] positions = new Vector3[numberStravaCoords];

			int count = _spawnedObjects.Count;

			if (counter < (float)numberStravaCoords) {
				counter += .1f;// / renderSpeed;
				Debug.Log (counter);
			}

			for (int i = 0; Convert.ToSingle(i) < counter; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location);
				positions[i] = _map.GeoToWorldPosition(location);
			}

			for (int i = (int)counter; i < numberStravaCoords; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
				positions[i] = _map.GeoToWorldPosition(location);			
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
}
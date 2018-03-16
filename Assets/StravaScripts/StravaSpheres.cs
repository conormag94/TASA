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

		void Start()
		{
			string url = "http://127.0.0.1:8010/stream";
			string activity_stream = Get(url);
			Debug.Log (activity_stream);
			//		LatLng latlng = LatLng.CreateFromJSON (activity_stream);
			var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>>(activity_stream);
			Debug.Log (locations[0][0]);

			_locations = new Vector2d[numberStravaCoords];
			_spawnedObjects = new List<GameObject>();

			for (int i = 0; i < numberStravaCoords; i++) // _locationStrings.Length
			{
				var locationString = "" + locations[i][0] + ", " + locations[i][1]; // "53.347440, -6.254188"
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i]);
				instance.transform.localScale = Vector3.one * _spawnScale;
				_spawnedObjects.Add(instance);
			}
		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location);
				Debug.Log (spawnedObject.transform.localPosition);
			}
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
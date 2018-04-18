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
	using Mapbox.Unity.MeshGeneration.Data;
	using Mapbox.Map;

	public class StravaSpheres : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 1f;

		[SerializeField]
		GameObject _markerPrefab;

		int numberStravaCoords = 134;

		List<GameObject> _spawnedObjects;

		private float counter;

		public float renderSpeed = 100f;

		List<float> altitudes;

		void Start()
		{
			string locations_str = GetStravaCoords();
			var locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<double>>> (locations_str);
			Debug.Log (locations [0] [0]);

			string altitudes_str = GetStravaAltitudes();
			altitudes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<float>> (altitudes_str);
			Debug.Log (altitudes[0]);

			_locations = new Vector2d[numberStravaCoords];
			_spawnedObjects = new List<GameObject>();

//			Debug.Log (_map.WorldRelativeScale);

			for (int i = 0; i < numberStravaCoords; i++)
			{

				var locationString = "" + locations[i][0] + ", " + locations[i][1];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
//				Debug.Log("start local scale before");
//				Debug.Log (instance.transform.localScale.x);
//				Debug.Log (instance.transform.localScale.y);
//				Debug.Log (instance.transform.localScale.z);

//				Vector3 testHeight = spawnPrefabWithHeight(_locations[i].x, _locations[i].y);
//				Debug.Log("test");
//				Debug.Log (testHeight);


//				Vector3 temp = _map.GeoToWorldPosition(_locations[i]);

				Debug.Log (_map.WorldRelativeScale);

				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], false);


//				Debug.Log (instance.transform.localPosition);
				instance.transform.localScale = Vector3.one * _spawnScale;

//				Debug.Log("start local scale");
//				Debug.Log (instance.transform.localScale.x);
//				Debug.Log (instance.transform.localScale.y);
//				Debug.Log (instance.transform.localScale.z);
				_spawnedObjects.Add(instance);

			}
		}
			
		private void Update()
		{
			Vector3[] positions = new Vector3[numberStravaCoords];

			int count = _spawnedObjects.Count;

			if (counter < (float)numberStravaCoords) {
				counter += 1f;// / renderSpeed;
			}

			for (int i = 0; Convert.ToSingle(i) < counter; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];

				Vector3 temp = _map.GeoToWorldPosition(location);
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, false); //new Vector3 (temp.x, (_map.WorldRelativeScale * altitudes[i]) - 0.15f, temp.y);

				Debug.Log("x");
				Debug.Log (spawnedObject.transform.localPosition.x);
//				Debug.Log("y");
//				Debug.Log (spawnedObject.transform.localPosition.y);
				Debug.Log("z");
				Debug.Log (spawnedObject.transform.localPosition.z);
//				spawnPrefabWithHeight(temp.x, temp.z);
				positions[i] = _map.GeoToWorldPosition(location);
			}

			for (int i = (int)counter; i < numberStravaCoords; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = new Vector3(10.0f, 10.0f, 10.0f);
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

		public string GetStravaAltitudes()
		{
			string url = "http://127.0.0.1:8010/altitude";
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


//		Vector3 spawnPrefabWithHeight(double lat, double lon)
//		{
//			//get tile ID
//			var tileIDUnwrapped = TileCover.CoordinateToTileId(new Mapbox.Utils.Vector2d(lat, lon), (int)_map.Zoom);
//
////			UnwrappedTileId utid = new UnwrappedTileId (12, 2116, 1473);
//
//			//get tile
//			UnityTile tile = _map._mapVisualizer.GetUnityTileFromUnwrappedTileId(tileIDUnwrapped);
//
//			//lat lon to meters because the tiles rect is also in meters
//			Vector2d v2d = Conversions.LatLonToMeters(new Mapbox.Utils.Vector2d(lat, lon));
//			//get the origin of the tile in meters
//			Vector2d v2dcenter = tile.Rect.Center - new Mapbox.Utils.Vector2d(tile.Rect.Size.x / 2, tile.Rect.Size.y / 2);
//			//offset between the tile origin and the lat lon point
//			Vector2d diff = v2d - v2dcenter;
//
//			//maping the diffetences to (0-1)
//			float Dx = (float)(diff.x / tile.Rect.Size.x);
//			float Dy = (float)(diff.y / tile.Rect.Size.y);
//
//			//height in unity units
//			var h = tile.QueryHeightData(Dx,Dy );
//
//			//lat lon to unity units
//			Vector3 location = Conversions.GeoToWorldPosition(lat, lon, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
//			//replace y in position
//			location = new Vector3(location.x, h, location.z);
//
//			Debug.Log (location.x);
//			Debug.Log (h);
//			Debug.Log (location.z);
//
//			return location;
//		}

	}
}
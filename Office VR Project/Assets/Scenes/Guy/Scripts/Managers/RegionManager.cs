using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour {

	public static RegionManager regionManager;
	public List<Camera> camera;

	[Header("Disabled Options:")]
	public LayerMask cannotTeleport;
	public LayerMask disabled;

	[Header("Enabled Options:")]
	public LayerMask canTeleport;
	public LayerMask enabled;

	[System.Serializable]
	public struct RegionData{
		public GameObject regionTrigger;
		public GameObject regionFlooring;

		public void CreateRegion(GameObject _RegionTrigger, GameObject _RegionFlooring) { //To create a region during playtime;
			regionTrigger = _RegionTrigger;
			regionFlooring = _RegionFlooring;
		}
	}

	public List<RegionData> regions;

	public static RegionManager Initialze {
		get {
		if(regionManager == null) {
			regionManager = FindObjectOfType(typeof(RegionManager)) as RegionManager;
		}

		if(regionManager == null) {
			GameObject r_Manager = new GameObject("RegionManager");
			regionManager = r_Manager.AddComponent(typeof(RegionManager)) as RegionManager;
			Debug.Log("Could not find manager, one was generated Automatically;");
		}
			
			return regionManager;
		}
	}

	private void OnApplicationQuit() {
		regionManager = null;
	}

	public void LoadRegion(int _RoomID) {
		foreach(RegionData _Region in regions) {
			_Region.regionTrigger.layer = disabled;
			_Region.regionFlooring.layer = cannotTeleport;
		}

		if(regions.Count - 1 >= _RoomID) {
			regions[_RoomID].regionTrigger.layer = enabled;
			regions[_RoomID].regionFlooring.layer = canTeleport;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionManager : MonoBehaviour {

	public bool fade;

	public static RegionManager regionManager;

	[Header("UI Settings:")]
	public Image overlay;
	public Color alpha;
	public float fadeTime = 1;

	[System.Serializable]
	public struct RegionData{
		public GameObject region;
		public GameObject[] regionFlooring;

		public void CreateRegion(GameObject _Region, GameObject[] _RegionFlooring) { //To create a region during playtime;
			region = _Region;
			regionFlooring = _RegionFlooring;
		}
	}

	public List<RegionData> regions;

	public static RegionManager Initialize {
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

	public void Awake() {
		regionManager = Initialize;
	}

	private void OnApplicationQuit() {
		regionManager = null;
	}

	public void LoadRegion(int _RoomID) {
		foreach(RegionData _Region in regions) { //Goes through every existing region;
			foreach(GameObject _Flooring in _Region.regionFlooring) { //And takes every flooring there;
			_Flooring.layer = 10; //And deactivate the ability to teleport on it;
			}

			_Region.region.SetActive(false);
		}

		if(regions.Count - 1 >= _RoomID) {
			foreach(GameObject _Flooring in regions[_RoomID].regionFlooring) {
			_Flooring.layer = 8;
			}
			regions[_RoomID].region.SetActive(true);
		}
	}

	public void Update() {
		overlay.color = alpha;

		Fade();
	}

	public void Fade() {
		switch(fade) {
			case false:
			alpha.a -= Mathf.Clamp01(Time.deltaTime / fadeTime);
			break;

			case true:
			alpha.a += Mathf.Clamp01(Time.deltaTime / fadeTime);
			break;
		}

		alpha.a = Mathf.Clamp01(alpha.a);
	}
}

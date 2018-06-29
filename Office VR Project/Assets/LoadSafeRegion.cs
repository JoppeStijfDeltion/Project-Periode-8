using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSafeRegion : MonoBehaviour {

	[Header("Influenced Properties:")]
	public Safedoor safe;	
	public bool hasLoaded = false; //If the player has already entered the region before;

	#region Private Variables
	BoxCollider _RegionLockArea;
	#endregion


	public void Update() {
        _RegionLockArea = GetComponent<BoxCollider>();

		if(hasLoaded == false) {
			if(safe.opened == true) {
			if(_RegionLockArea.bounds.Contains(GameManager.gameManager.player.transform.position)) {
				hasLoaded = true;
				safe.CloseDoor(); //Closes the safedoor;
				RegionManager.regionManager.LoadRegion(2); //Loads the region;
				}
			}
		}
	}	
}

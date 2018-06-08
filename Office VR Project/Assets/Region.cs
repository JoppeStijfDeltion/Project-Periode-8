using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script is used to make the player immobile when they enter grounds where they shouldn't be at*/

public class Region : MonoBehaviour {

	public void OnTriggerStay(Collider _Col) {
		print(_Col);

		if(_Col.transform.tag == "Player") {

			if(GameManager.gameManager.startedGame == true) { //IF the same has started;
			RegionManager.regionManager.fadeTime = 5;
			RegionManager.regionManager.fade = false;
			}
		}
	}

	public void OnTriggerExit(Collider _Col) {
		print(_Col);

		if(_Col.transform.tag == "Player") { //IF the camera is within range of the loading region;
			if(GameManager.gameManager.startedGame == true) { //IF the same has started;
			RegionManager.regionManager.fadeTime = 1;
			RegionManager.regionManager.fade = true;
			}
		}
	}
}

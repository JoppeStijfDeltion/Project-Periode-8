using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script is used to make the player immobile when they enter grounds where they shouldn't be at*/

public class Region : MonoBehaviour {

	#region Private Variables
	Collider[] colliders; //All the colliders recorded before the first frame;
	#endregion

	private void Awake() {
		colliders = GetComponents<Collider>(); //Tracks all the colliders of this object at initialization;
	}

	private void Update() {
		if(GameManager.gameManager.startedGame == true) { //If the game has started;
		RegionManager.regionManager.fadeTime = 0.3f;
		RegionManager.regionManager.fade = CheckForFade(colliders); //Check if the player is within bounds;
		}
	}

	public bool CheckForFade(Collider[] _Colliders) {
		foreach(Collider _Col in _Colliders) { //For every collider on this object;
			if(_Col.bounds.Contains(GameManager.gameManager.player.transform.position)) { //Even if only one of them contains the player;
				print("Player contained in: "+gameObject.name);
				return false; //Prevent fading
			}
		}

		print("Nothing contained in: " +gameObject.name);
		return true; //If no colliders contain any player whatsoever, fade the screen;
	}
}

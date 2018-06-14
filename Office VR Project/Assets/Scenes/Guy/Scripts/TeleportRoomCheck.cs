using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script is used to prevent teleporting through objects, and avoid general clipping with color based feedback*/

public class TeleportRoomCheck : MonoBehaviour {

	public static bool canTeleport = true;

	public Material parentCol; //Parent color;
	public Material childCol; //Child color;
	public Material lineCol; //Linecolor;

	[Header("test")]
	public Color parent;

	public void Update() {
		CustomTriggerArea(); //Custom written OnTrigger station;
	}

	private void CustomTriggerArea() {
		Collider[] _CurrentColliders = new Collider[10]; //Storage for all colliders within the triggerfield;
		Physics.OverlapBoxNonAlloc(transform.position, transform.GetComponent<BoxCollider>().size, _CurrentColliders); //Collecting all current data;

	if(_CurrentColliders != null) {
		foreach(Collider _Col in _CurrentColliders) { //For every collider that is within the trigger field;
			print(_Col);
			if(_Col != null) { //If the collider has data;
			if(_Col.isTrigger == false) { //If the collider is NOT a triggerfield;
				if(_Col.gameObject.layer != 8) { //If the collider currently is NOT a teleport allowing mask;

				parentCol.color = Color.red; //Gives the player negative feedback;
				childCol.color = Color.red;
				lineCol.color = Color.red;
				canTeleport = false; //Stops the user from teleporting;
				return; //Cuts off the function due to the circumstances not meeting up to the criteria stated above;
				}
			}
		}
	}
}

		parentCol.color = Color.green; //Gives positive feedback;
		childCol.color = Color.green;
		lineCol.color = Color.green;
		canTeleport = true; //Allows the player to teleport;
	}
}

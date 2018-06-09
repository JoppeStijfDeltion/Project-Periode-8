using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRoomCheck : MonoBehaviour {

	public bool canTeleport = false;

	private void OnTriggerStay(Collider _Col) {
		if(_Col.isTrigger == false) {
			if(_Col.transform.gameObject.layer != 8) {
			canTeleport = false;
			GetComponent<MeshRenderer>().material.color = Color.red;
			}

			else if(_Col.transform.gameObject.layer == 8)
			canTeleport = true;
			GetComponent<MeshRenderer>().material.color = Color.green;
		}
	}
}

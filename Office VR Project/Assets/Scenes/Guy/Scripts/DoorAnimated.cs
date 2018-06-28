using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : InteractableObject {

	private bool opened = false;

	public override void Interact() {
		if(opened == false && hand != null) {
			opened = true;
			anim.SetTrigger("Open");
		}
	}
}

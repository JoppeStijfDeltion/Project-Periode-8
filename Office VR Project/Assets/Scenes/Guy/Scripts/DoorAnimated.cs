using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : InteractableObject {

    [Header("Special Door Options:")]
    public bool isFusebox = false;

	private bool opened = false;

	public override void Interact() {
		if(opened == false && hand != null) {
			opened = true;
			anim.SetTrigger("Open");
            if (isFusebox == true)
                Fusebox.fuseBox.started = true;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*One of the fuses that can be either activated or deactivated*/

public class Fuse : InteractableObject {

	[Header("Influenced Fuses:")]
	public Fuse[] InfluencedFuses;
	public bool activated = false;

	public void Awake() {
		switch(activated) {
			case true:
			activated = false;
			GetComponent<MeshRenderer>().material.color = Color.red;
			break;

			case false:
			activated = true;
			GetComponent<MeshRenderer>().material.color = Color.green;
			break;
		}
	}

	public override void Update(){return;}

	public override void Grapple(PickupSystem _Object) {
		{
		if(Input.GetButtonDown("Fire1")) {
			foreach(Fuse fuse in Fusebox.fuseBox.fuses) {
				if(fuse.hand != null) 
				fuse.hand = null;
			}

			Interact();
		}
	}
}

	public override void Interact() {
		if(Fusebox.fuseBox.completed)
		return;

		switch(activated) {
			case true:
			activated = false;
			GetComponent<MeshRenderer>().material.color = Color.red;
			break;

			case false:
			activated = true;
			GetComponent<MeshRenderer>().material.color = Color.green;
			break;
		}

		ActivateFuses();
	}

	public void ActivateFuses() {
		foreach(Fuse f in InfluencedFuses) {
				switch(f.activated) {
					case true:
					f.activated = false;
					f.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
					break;

					case false:
					f.activated = true;
					f.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
					break;
				}		

			Fusebox.fuseBox.Completion();
			}
	}

	
	#region Physics


	public void OnTriggerStay(Collider c) {
		bool detectedHand = InteractionCheck(c.gameObject);

		if(detectedHand)
		hand = c.gameObject.GetComponent<PickupSystem>();
		else
		return;

		if(hand != null) {
			Grapple(hand);
		}
	}

	
	public override void OnTriggerExit(Collider c) {
	//base.OnTriggerExit(null);
	if(hand != null)
		if(c.gameObject == hand.gameObject)
		hand = null;
	}


	#endregion

	#region Checks
	static bool InteractionCheck(GameObject _Object) {
		if(_Object.GetComponent<PickupSystem>())
			if(_Object.GetComponent<PickupSystem>().objectBeingCarried == null)
			return true;

			return false;
	}
	#endregion
}

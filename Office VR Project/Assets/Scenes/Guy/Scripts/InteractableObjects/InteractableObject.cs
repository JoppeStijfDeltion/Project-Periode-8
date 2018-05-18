using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Objects with these scripts tend to have a different function than being picked up */

public abstract class InteractableObject : MonoBehaviour {

	[Header("Current Status:")]
	public PickupSystem hand;

	public virtual void Update() {
		if(hand != null)
		{
		Interact();
		}
	}

	public virtual void DrawOutline(bool _DrawOutline) {
		/*if(_DrawOutline) 
			gameObject.GetComponent<MeshRenderer>().materials[1].SetFloat("_Outline", 0.01f);
   		 else
			gameObject.GetComponent<MeshRenderer>().materials[1].SetFloat("_Outline", 0);*/
	}

	public abstract void Grapple(PickupSystem _Object);

	public abstract void Interact();

	public virtual void OnTriggerEnter(Collider c) {
		if(c.GetComponent<PickupSystem>())
			DrawOutline(true);
	}

	public virtual void OnTriggerExit(Collider c) {
		if(c.GetComponent<PickupSystem>())
			DrawOutline(false);
	}
}

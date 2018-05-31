using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour {
	
	private Vector3 scale = new Vector3(1, 1, 1);

	public float distanceBelowTillFriction;
	public bool canChild = true;

	private void OnCollisionEnter(Collision c) { //This function is used so that objects move along with the object below;
		Ray below = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		Vector3 newelyFoundScale = Vector3.zero;

	if(canChild == true) {
		if(Physics.Raycast(below, out hit, distanceBelowTillFriction)) { 
			if(!hit.transform.GetComponent<PickupSystem>())
			transform.SetParent(hit.transform, true); 			

			}
		}
	}
}

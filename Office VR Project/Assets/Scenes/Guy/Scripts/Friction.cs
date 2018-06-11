using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour {
	
	[HideInInspector]
	public float distanceBelowTillFriction;
	
	public bool canChild = true;

	private void OnCollisionEnter(Collision c) { //This function is used so that objects move along with the object below;
		Ray below = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		Vector3 newelyFoundScale = Vector3.zero;

	if(canChild == true) {
		if(Physics.Raycast(below, out hit, distanceBelowTillFriction)) { 
			if(!hit.transform.GetComponent<PickupSystem>() && hit.transform.tag != "Player")
			transform.SetParent(hit.transform, true); 			
			}
		}

		if(KineticEnergy(GetComponent<Rigidbody>()) > 0.5f) {
			AudioManager.audioManager.PlayAudio(AudioManager.audioManager.collision, transform);
		}
	}

	public static float KineticEnergy(Rigidbody _Rigidbody){
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f*_Rigidbody.mass*Mathf.Pow(_Rigidbody.velocity.magnitude,2);
    }
}

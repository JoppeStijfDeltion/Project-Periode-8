using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Used for drawing a selected shader*/

public abstract class AllInteractables : MonoBehaviour {

	protected PickupSystem detectedHand; //The object that it has tracked;

	public virtual void OnTriggerEnter(Collider c) { //If the object collides with an triggerfield;
		if(c.transform.GetComponent<PickupSystem>()) { //If the objecti it is colliding with has a pickupsystem;
			detectedHand = c.GetComponent<PickupSystem>();
			GetComponent<MeshRenderer>().material.SetFloat(("Interactable"), 1); //Set the material to its selected format;
		}
	}

	public virtual void OnTriggerExit(Collider c) { //If the object leaves a trigger field;
			if(c.transform.gameObject == detectedHand.gameObject) { //If the object was a hand;
			GetComponent<MeshRenderer>().material.SetFloat(("Interactable"), 0); //Set the material to its selected format;
			}
		}
	}

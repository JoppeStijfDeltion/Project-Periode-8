using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script has the primary use of enabling the player to interact with props and different objects*/

[RequireComponent(typeof(Rigidbody), typeof(FixedJoint))]
public class PickupSystem : MonoBehaviour {

	[Header("Debug Settings")]
	public Pickup objectBeingCarried = null;

	[Tooltip("This is the interval between 2 stored positions to decide the throw force.")]
	public int framesTillPositionStore = 15;
	public float throwforceMultiplier = 2;

	#region Private Variables
	private FixedJoint thisJoint; //Current gameobject its fixed joint;

	private Rigidbody otherBody;
	private Rigidbody thisBody;

	private bool isThrowing = false;
	private Pickup currentObjectWithinRange = null;

	private Vector3 newLocation;
	private Vector3 oldLocation;

	private int frameCount;
	#endregion
	
	public void Awake() { //Sets some references;
		thisJoint = GetComponent<FixedJoint>(); //Sets the main joint of this object;
		thisBody = GetComponent<Rigidbody>();
	}

	private void Update() {
		ObjectInteraction(); 
	}

	private void FixedUpdate() {
		StorePosition();
	}

	bool CanCarry() {
		if(objectBeingCarried == null)
			if(currentObjectWithinRange != null)
				if(Input.GetButtonDown("Fire1"))
					return true;
					
					//If the statements above are not met, it will return false;
					return false;
	}

	private void Throwing() {
			if(objectBeingCarried != null)
			{		
			otherBody.velocity = (newLocation - oldLocation) * throwforceMultiplier;
			objectBeingCarried.beingCarried = false;
			thisJoint.connectedBody = null;
			objectBeingCarried = null;
		}
	}

	private void StorePosition() {
		newLocation = transform.position;

		if(framesTillPositionStore < 15)
		framesTillPositionStore++;
		else {
			oldLocation = transform.position;
			framesTillPositionStore = 0;
		}

	}

    private void ObjectInteraction() {
		bool canCarry = CanCarry(); 

				if(canCarry) {
					objectBeingCarried = currentObjectWithinRange;	
					objectBeingCarried.beingCarried = true;							
					thisJoint.connectedBody = objectBeingCarried.GetComponent<Rigidbody>();
				    return;
				} 

				if(Input.GetButtonUp("Fire1"))
					Throwing();
				}

	private void OnTriggerEnter(Collider c) {

		if(c.transform.GetComponent<Pickup>())	{
			if(c.transform.GetComponent<Pickup>().beingCarried == false)
			currentObjectWithinRange = c.transform.gameObject.GetComponent<Pickup>();
			otherBody = currentObjectWithinRange.gameObject.GetComponent<Rigidbody>();
		}

	}

	private void OnTriggerExit(Collider c) {
		currentObjectWithinRange = null;
	} 
}

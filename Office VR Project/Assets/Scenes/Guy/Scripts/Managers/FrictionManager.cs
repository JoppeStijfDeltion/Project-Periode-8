using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionManager : ManagerManager {

	public static FrictionManager frictionManager;

	public float frictionDistance;

	public override void Initialization() {
		if(frictionManager == null) { //If no reference has been made;
			frictionManager = this; //This is the reference;
			return; //Cut off function
		} 

		Destroy(this); //Else destroy this;
	}
 
	// Use this for initialization
	void Start () {
		Rigidbody[] physicsObjects = FindObjectsOfType<Rigidbody>();

		foreach(Rigidbody _PhysicObject in physicsObjects) {
			if(_PhysicObject.isKinematic == false) {
				_PhysicObject.transform.gameObject.AddComponent<Friction>();

				Friction _Friction = _PhysicObject.transform.gameObject.GetComponent<Friction>();
				_Friction.distanceBelowTillFriction = frictionDistance;
			}
		}
	}
}

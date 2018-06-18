using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionManager : ManagerManager {

	public static FrictionManager frictionManager;
	
	[Header("Ignored Objects:")]
	public Rigidbody[] mask;
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
				bool _IsMasked = MaskCheck(_PhysicObject); //Runs a check;

				if(_IsMasked == false) {
				_PhysicObject.transform.gameObject.AddComponent<Friction>();
				Friction _Friction = _PhysicObject.transform.gameObject.GetComponent<Friction>();
				_Friction.distanceBelowTillFriction = frictionDistance;
				}
			}
		}
	}

	bool MaskCheck(Rigidbody _ObjectFound) {
			foreach(Rigidbody _Mask in mask) { //Go through every masked object;
				if(_ObjectFound == _Mask) { //If the object is the same as one of the masked objects;
					return true; //Return false;
				}
			}

		return false;
	}
}

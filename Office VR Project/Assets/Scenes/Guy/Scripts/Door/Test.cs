using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Physics door test*/

public class Test : InteractableObject {

	[Header("Options:")]
	public GameObject hinge; //Object where the fixed joint is connected to;
	public float minRot = 0, maxRot = 90; //Rotation clamping;

	 void Start() {
		JointLimits _Limits = hinge.GetComponent<HingeJoint>().limits;
		_Limits.min = minRot; //Dit is ook gay;
		_Limits.max = maxRot; //Dit is gay
		hinge.GetComponent<HingeJoint>().limits = _Limits;
		hinge.GetComponent<HingeJoint>().useLimits = true;
	}

	public override void Interact() { /*Main function to interact*/
		if(hand == null) {
		 anim.SetBool("Selected", false);
		 return;
		 }
		
		 anim.SetBool("Selected", true);

	    Vector3 _TargetRotation = hinge.transform.position - hand.transform.position; //Calculate rotate target;
	    _TargetRotation.y = 0; //Disclaims the Y axis out of the rotation target;
	    float _AngleDif = Vector3.Angle(transform.forward, _TargetRotation); //Calculates what the next angle is to move towards to;
	    Vector3 _CrossIntersection = Vector3.Cross(transform.forward, _TargetRotation); //Defines what the distance is between the next three dimensional angle;
	    hinge.GetComponent<Rigidbody>().angularVelocity = _CrossIntersection * _AngleDif * 50; //Calculates the final angular velocity;

		
		transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, minRot, maxRot), transform.eulerAngles.y, transform.eulerAngles.z);

	}

	public override void Update() {}

	private void FixedUpdate() {
		Interact();
	}
}

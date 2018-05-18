using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject {

	[Header("Door Options")]
	public GameObject hinge;
	public float minRotation;
	public float maxRotation;
	public float followingspeed = 2;

	public bool locked = false;

	#region Private Variables
	private Animator anim;
	private Vector3 currentRotation;
	#endregion

	private void Awake() {
		anim = GetComponent<Animator>();
		currentRotation = hinge.transform.eulerAngles;
	}

	public override void Update() {
		base.Update();
		hinge.transform.eulerAngles = currentRotation;
	}

	public override void Grapple(PickupSystem _Object) {
		if(Input.GetButtonDown("Fire1")) {
			anim.SetTrigger("Down");
		hand = _Object;
		}
	}

	public override void Interact() {
		if(hand == null) { return; } //If there is no object to track, function will be cut off;

			Quaternion targetRot = Quaternion.LookRotation(hand.transform.position - hinge.transform.position);
			currentRotation.y = Mathf.Lerp(currentRotation.y, ClampAngle(targetRot.eulerAngles.y), followingspeed * Time.deltaTime);

			if(Input.GetButtonUp("Fire1"))
			{
			hand = null;
			anim.SetTrigger("Up");
			}
		}

	#region Physics

	private void OnTriggerStay(Collider c) {
		bool detectedHand = InteractionCheck(c.gameObject);

		if(detectedHand)
		Grapple(c.gameObject.GetComponent<PickupSystem>());
		Interact();
	}

	public override void OnTriggerExit(Collider c) {
		base.OnTriggerExit(null);
		if(hand != null)
			if(c.transform == hand.transform)
			hand = null;
	}

	private float ClampAngle(float angle) {

		if (angle < 90 || angle > 270) {
		if (angle > 180) angle -= 360;
		if (maxRotation > 180) maxRotation -= 360;	
		if (minRotation > 180) minRotation -= 360;
		} 

		angle = Mathf.Clamp(angle, minRotation, maxRotation);

		if (angle < 0) angle += 360;
		return angle;
}

	#endregion

	#region Checks
	bool InteractionCheck(GameObject _Object) {
		if(_Object.GetComponent<PickupSystem>())
			if(_Object.GetComponent<PickupSystem>().objectBeingCarried == null)
				if(locked == false)
			return true;

			return false;
	}
	#endregion
}

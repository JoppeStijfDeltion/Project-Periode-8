using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This object is only affected by one axis based on the object its grasped onto */

public class Drawer : InteractableObject {

	public Vector3 currentLocation;

	[Header("Drawer Settings:")]
	public float maxOpenOffset = 0.8f;
	public float pivotOffet = 2;
	public float drawSpeed = 1;

	#region Private Variables
	/*Drawer Related*/
	private float startOffsetZ;

	/*Hand Related*/
	private bool correctedOffset = false;
	#endregion

	private void Awake() {
		currentLocation = transform.localPosition; //Correcting some transforms;
		startOffsetZ = transform.localPosition.z;
	}

	public override void Update() {
		base.Update();
		transform.localPosition = currentLocation;
	}

	public override void Grapple(PickupSystem _Object) {
		if(Input.GetButtonDown("Fire1"))
		hand = _Object;
	}

	public override void Interact() {
		if(hand == null) { correctedOffset = false; return; } //If there is no object to track, function will be cut off;

		if(correctedOffset == false) {
			correctedOffset = true;
		}

		currentLocation.z = Mathf.Lerp(currentLocation.z, transform.InverseTransformDirection(hand.transform.position).z + pivotOffet, drawSpeed * Time.deltaTime);
		currentLocation.z = Mathf.Clamp(currentLocation.z, startOffsetZ, startOffsetZ + maxOpenOffset);

		if(Input.GetButtonUp("Fire1"))
		hand = null;

		}

	#region Physics

	public void OnTriggerStay(Collider c) {
		bool detectedHand = InteractionCheck(c.gameObject);

		if(detectedHand)
		Grapple(c.gameObject.GetComponent<PickupSystem>());
		Interact();
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

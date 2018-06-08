using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screws : MonoBehaviour {

	public GameObject screwdriver;
	public Vector3 pos;

	private void OnTriggerEnter (Collider col) {
		if (col.gameObject == screwdriver) {
			Destroy (screwdriver.GetComponent<Rigidbody> ());
			col.transform.SetParent (transform, true);
			//play animation
			print ("play screw animation");
		}
	}

	public void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Drop ();
		}
	}

	public void Drop () {

		screwdriver.transform.SetParent (null);
		screwdriver.AddComponent<Rigidbody> ();
		Rigidbody rb = gameObject.AddComponent<Rigidbody> ();
		rb.isKinematic = false;
		rb.useGravity = true;
	}

	private void OnDisabled () {
		Destroy (gameObject.GetComponent<Rigidbody> ());
	}

}
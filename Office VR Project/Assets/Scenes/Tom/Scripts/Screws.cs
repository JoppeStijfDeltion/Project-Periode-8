using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screws : MonoBehaviour {

	public GameObject screwdriver;
	private Rigidbody rb;

	private void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.useGravity = false;
	}

	private void OnTriggerEnter (Collider col) {
		if (col.gameObject == screwdriver) {
			rb.isKinematic = false;
			rb.useGravity = true;
		}
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour {

	public GameObject screwdriver;
	private Transform parent;
	private BoxCollider colRef;
	private Vent vent;

	private void Start () {
		colRef = GetComponent<BoxCollider> ();
		//screwdriver = GameObject.FindWithTag ("Screwdriver");
		parent = screwdriver.transform.parent;
		vent = GameObject.FindWithTag ("Vent").GetComponent<Vent> ();
	}

	public void Update () {
		if (colRef.bounds.Contains (screwdriver.transform.position)) {
			if (parent.parent != transform) {
				Destroy (parent.GetComponent<Rigidbody> ());
				parent.GetComponent<BoxCollider> ().enabled = false;
				parent.parent = transform;
				parent.localPosition = new Vector3 (0, 0, 0.4355f);
				parent.localEulerAngles = new Vector3 (0, 180, 0);
				GetComponent<Animator> ().SetTrigger ("Start");
			}
		}
	}

	public void Drop () {
		vent.Check ();
		parent.parent = null;
		parent.GetComponent<BoxCollider> ().enabled = true;
		parent.gameObject.AddComponent<Rigidbody> ();
		gameObject.GetComponent<Animator> ().enabled = false;
		gameObject.AddComponent<Rigidbody> ();
		Destroy (GetComponent<BoxCollider> ());
		gameObject.AddComponent<BoxCollider> ();
		transform.parent = null;
		Destroy (this);
	}
}
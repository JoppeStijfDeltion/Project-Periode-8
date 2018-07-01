using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mold : MonoBehaviour {

	public Transform keyPartOne;
	public Transform keyPartTwo;
	public Transform key;

	public bool one;
	public bool two;
	public bool completed;

	private void OnTriggerEnter (Collider col) {
		if (col.transform == keyPartOne) {
			col.transform.parent = transform.GetChild (0);
			col.transform.localPosition = Vector3.zero;
			col.transform.localEulerAngles = Vector3.zero;
			Destroy (col.GetComponent<Rigidbody> ());
			Destroy (col.GetComponent<Friction> ());
			col.GetComponent<Collider> ().enabled = false;
			one = true;

		}
		else if (col.transform == keyPartTwo) {
			col.transform.parent = transform.GetChild (1);
			col.transform.localPosition = Vector3.zero;
			col.transform.localEulerAngles = Vector3.zero;
			Destroy (col.GetComponent<Rigidbody> ());
			Destroy (col.GetComponent<Friction> ());
			col.GetComponent<Collider> ().enabled = false;
			two = true;
		}
		if (one && two && !completed) {
			completed = true;
			Destroy (keyPartOne.gameObject);
			Destroy (keyPartTwo.gameObject);
			Transform _key = Instantiate (key, transform.GetChild (2));
			_key.transform.localPosition = Vector3.zero;
			_key.transform.localEulerAngles = Vector3.zero;
			_key.gameObject.AddComponent<Rigidbody> ();
			_key.gameObject.AddComponent<BoxCollider> ();
			_key.gameObject.AddComponent<Friction> ();
		}
	}
}
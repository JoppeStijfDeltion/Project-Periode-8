using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeight : MonoBehaviour {

	[HideInInspector]
	public float mass;
	public WeightPuzzle puzzle;
	private List<Rigidbody> colliders = new List<Rigidbody> ();

	private void OnTriggerEnter (Collider collider) {
		if (collider.GetComponent<Rigidbody> ()) {
			colliders.Add (collider.GetComponent<Rigidbody> ());
		}
		UpdateMass ();
	}

	private void OnTriggerExit (Collider collider) {
		if (collider.GetComponent<Rigidbody> ()) {
			colliders.Remove (collider.GetComponent<Rigidbody> ());
		}
		UpdateMass ();
	}

	private void UpdateMass () {
		float m = 0;
		for (int i = 0; i < colliders.Count; i++) {
			m += colliders[i].mass;
		}
		mass = m;
		puzzle.UpdateMass ();
	}
}
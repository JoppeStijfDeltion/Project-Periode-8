using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPuzzle : MonoBehaviour {

	public float mass;
	public List<Collider> colliders = new List<Collider> ();

	private void OnTriggerStay (Collider collider) {
		if (!colliders.Contains (collider)) {
			colliders.Add (collider);
			UpdateMass ();
		}
	}

	private void UpdateMass () {
		for (int i = 0; i < colliders.Count; i++) {
			if (colliders[i].GetComponent<Rigidbody> ()!= null) {
				mass += colliders[i].GetComponent<Rigidbody> ().mass;
			}
		}
	}
}
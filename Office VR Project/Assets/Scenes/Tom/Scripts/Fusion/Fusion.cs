using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion : MonoBehaviour {

	public GameObject otherPart;
	public GameObject result;

	private void OnCollisionEnter (Collision _collision) {
		if (_collision.gameObject == otherPart) {
			Destroy (_collision.gameObject);
			Instantiate (result, _collision.transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
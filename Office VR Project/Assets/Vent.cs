using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour {

	public int numberOfScrews = 4;

	public void Check () {
		numberOfScrews--;
		if (numberOfScrews <= 0) {
			GetComponent<BoxCollider> ().enabled = true;
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Friction>();
			RegionManager.regionManager.LoadRegion(3);
		}
	}
}
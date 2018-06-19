using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour {

	public int numberOfScrews = 4;

	public void Check () {
		numberOfScrews--;
		if (numberOfScrews <= 0) {
			//Do something
		}
	}
}
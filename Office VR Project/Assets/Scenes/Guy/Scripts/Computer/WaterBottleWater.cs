using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottleWater : MonoBehaviour {

	void OnParticleCollision(GameObject _Col) {
		if(_Col.GetComponent<EmergencyDoor>())
			_Col.GetComponent<EmergencyDoor>().IncrementPower(2);
	}
}

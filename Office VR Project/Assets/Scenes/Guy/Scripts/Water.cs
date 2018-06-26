using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	[Header("Settings:")]
	public float incrementAmount = 0.05f;

	void OnParticleCollision(GameObject _Bottle) {
		if(_Bottle.transform.tag == "Water") {
			WaterBottle _WaterBottle = _Bottle.transform.parent.GetComponent<WaterBottle>();
			_WaterBottle.AddWater(incrementAmount);
			return;
		}
	}
}

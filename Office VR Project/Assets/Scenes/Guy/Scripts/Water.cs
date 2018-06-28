using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	[Header("Settings:")]
	public float incrementAmount = 0.05f;

	void OnParticleCollision(GameObject _Bottle) {
        print(_Bottle);
		if(_Bottle.transform.GetComponent<WaterBottle>()) {
			_Bottle.GetComponent<WaterBottle>().AddWater(incrementAmount);
            print("Incrementing water...");
			return;
		}
	}
}

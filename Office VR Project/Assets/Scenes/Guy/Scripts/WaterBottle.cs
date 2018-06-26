using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour {

	[Header("Visual Settings:")]
	public bool capped = true;
	public GameObject waterCylinder;
	public ParticleSystem water;
	public float minMass, maxMass;
	public float waterDecreaseSpeed = 0.01f;

	#region Private Variables
	Rigidbody rb;
	#endregion

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}

    void Update() {
		SprayWater();

		Vector3 _Waterscale = new Vector3(waterCylinder.transform.lossyScale.x, rb.mass, waterCylinder.transform.lossyScale.z);
		waterCylinder.transform.localScale = _Waterscale;
	    rb.mass = Mathf.Clamp(rb.mass, minMass, maxMass);
	}

	void SprayWater() {
		print(transform.eulerAngles.x);

	if(capped == false) {
		if(transform.eulerAngles.x > -95 &&  transform.eulerAngles.x < 95 && rb.mass > 0) {
			//water.Play();
			rb.mass -= waterDecreaseSpeed * Time.deltaTime;
		} /*else {
		if(water.isPlaying) { 
			water.Stop();
		}
	} */
	}
}

	public void AddWater(float _Increment) {
		rb.mass += (_Increment * Time.deltaTime);
	}
}

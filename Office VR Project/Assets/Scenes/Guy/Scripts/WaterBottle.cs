using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour {

	[Header("Visual Settings:")]
	public bool capped = true;
    public GameObject cap;

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
        if (transform.GetChild(0).transform.tag != "Cap") capped = false;
        SprayWater();

        Vector3 _Waterscale = new Vector3(waterCylinder.transform.lossyScale.x, rb.mass, waterCylinder.transform.lossyScale.z);
		waterCylinder.transform.localScale = _Waterscale;
	    rb.mass = Mathf.Clamp(rb.mass, minMass, maxMass);
	}

	void SprayWater() {

	if(capped == false) {
            if (transform.eulerAngles.x > -95 && transform.eulerAngles.x < 95 && rb.mass > minMass)
            {
                water.Play();
                rb.mass -= waterDecreaseSpeed * Time.deltaTime;
            }
            else
                water.Stop();
        }
        else
            water.Stop();
    }

	public void AddWater(float _Increment) {
		rb.mass += (_Increment * Time.deltaTime);
	}
}

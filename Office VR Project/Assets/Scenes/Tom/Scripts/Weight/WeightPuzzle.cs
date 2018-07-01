using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightPuzzle : MonoBehaviour {

	public float requiredMass = 1f;
	public GetWeight leftWeight;
	public GetWeight rightWeight;
	public Text leftText;
	public Text rightText;

	public EmergencyDoor door;

	private float currentMass;
	private float newMass;
	private Vector2 mass;
	private float t;

	private bool completed;

	private void Update () {
		if (t <= 1) {
			t += 0.5f * Time.deltaTime;
		}
		currentMass = Mathf.Lerp (currentMass, newMass, t);
		mass.x = Mathf.Lerp (mass.x, leftWeight.mass, t);
		mass.y = Mathf.Lerp (mass.y, rightWeight.mass, t);
		if (leftWeight.mass == rightWeight.mass && currentMass == requiredMass && !completed) {
			completed = true;
			Completed ();
		}
		leftText.text = mass.x.ToString ("F2");
		rightText.text = mass.y.ToString ("F2");
	}

	private void Completed () {
		if(door.opened == false)
		door.Open();
	}

	public void UpdateMass () {
		newMass = leftWeight.mass + rightWeight.mass;
		t = 0f;
	}
}
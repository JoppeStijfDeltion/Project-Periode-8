using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

	[Header("Clock Settings:")]
	public Transform secondHinge, hourHinge;
	public Vector3 secondHingeRot, hourHingeRot;

	private float updateVisualTimer = 1;

	public void Update() {
		SetClock();

		updateVisualTimer -= Time.deltaTime;
		if(updateVisualTimer <= 0) {
			updateVisualTimer = 1;
			UpdateVisuals();
		}
	}

	public void UpdateVisuals() {
		secondHinge.localEulerAngles = secondHingeRot;
		hourHinge.localEulerAngles = hourHingeRot;
	}

	public void SetClock() {
		secondHingeRot.x = (GameManager.gameManager.seconds + (GameManager.gameManager.tenSeconds * 10)) * (360 / 60);
		hourHingeRot.x = GameManager.gameManager.minutes * (360 / 60);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDispenser : InteractableObject {

	[Header("Settings:")]
	public ParticleSystem water;

	public override void Interact() {
		if(water.isPlaying == false)
			water.Play();
		else if(water.isPlaying == true)
			water.Stop();

			hand = null;
	}
}

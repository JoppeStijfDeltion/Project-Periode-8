﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*One of the fuses that can be either activated or deactivated*/

public class Fuse : InteractableObject {

	[Header("Influenced Fuses:")]
	public List<Fuse> InfluencedFuses = new List<Fuse>();
	public bool activated = false;

	public override void Awake() {
		base.Awake();
		SetStartingColors();
	}

	private void SetStartingColors() { //When the game initializes, it updates its visuals based on its starting parameters;
		switch(activated) {
			case true:
			GetComponent<MeshRenderer>().material.color = Color.green; //If NOT activated;
			break;

			case false:
			GetComponent<MeshRenderer>().material.color = Color.red; //If activated;
			break;
		}
	}

	public override void Interact() {
		if(Fusebox.fuseBox.completed)
		return;

		AudioManager.audioManager.PlayAudio(aSource, sounds[0]);
		anim.SetTrigger("Press");
		switch(activated) {
			case true:
			activated = false;
			GetComponent<MeshRenderer>().material.color = Color.red;
			break;

			case false:
			activated = true;
			GetComponent<MeshRenderer>().material.color = Color.green;
			break;
		}

		ActivateFuses();
	}

	public void ActivateFuses() {
		foreach(Fuse f in InfluencedFuses) {
				switch(f.activated) {
					case true:
					f.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
					f.activated = false;
					break;

					case false:
					f.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
					f.activated = true;
					break;
				}		
			}

			Fusebox.fuseBox.Completion();
			hand = null;
	}
}
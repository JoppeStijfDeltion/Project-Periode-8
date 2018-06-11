using System.Collections;
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
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
			break;

			case false:
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
			break;
		}
	}

	public override void Interact() {
		if(Fusebox.fuseBox.completed)
		return;

		AudioManager.audioManager.PlayAudio(sounds[0], transform);
		anim.SetTrigger("Press");
		switch(activated) {
			case true:
			activated = false;
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
			break;

			case false:
			activated = true;
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
			break;
		}

		ActivateFuses();
	}

	public void ActivateFuses() {
		foreach(Fuse f in InfluencedFuses) {
				switch(f.activated) {
					case true:
					f.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
					f.activated = false;
					break;

					case false:
					f.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
					f.activated = true;
					break;
				}		
			}

			Fusebox.fuseBox.Completion();
			hand = null;
	}
}

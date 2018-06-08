using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : InteractableObject {

	[Header("Influenced Properties:")]
	public Safedoor safe;	

	#region Private Variables
	private bool alreadyInteracted = false; //If the button has been activated yet;
	#endregion

	public override void Interact() {
		anim.SetTrigger("Press"); //Plays the buttonpress animation;
		AudioManager.audioManager.PlayAudio(sounds[0], transform); //Plays the button press sound;

		if(alreadyInteracted != false) { return; } //If the button has already been interacted with, return;
		alreadyInteracted = true;	//Indentifies the button as *interacted*
		safe.anim.SetTrigger("Close"); //Closes the safedoor;
		RegionManager.regionManager.LoadRegion(1);
		//Play alarm sound;
		//Lock region;
	}
}

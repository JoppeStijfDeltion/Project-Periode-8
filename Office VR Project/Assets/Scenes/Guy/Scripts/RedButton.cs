using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : InteractableObject {

	[Header("Public Alarm Settings:")]
	public GameObject alarmLight;
	public float rotationSpeed;
	public AudioSource alarmAudioSource;

	#region Private Variables
	private bool alreadyInteracted = false; //If the button has been activated yet;
	#endregion

	public override void Interact() {
		if(alreadyInteracted == true) { return; } //If the button has already been interacted with, return;

		alreadyInteracted = true;	//Indentifies the button as *interacted*
		if(alarmAudioSource != null)
		alarmAudioSource.Play();
		print("ALARM");
		anim.SetTrigger("Press"); //Plays the buttonpress animation;
		AudioManager.audioManager.PlayAudio(sounds[0], transform); //Plays the button press sound;
	}

	public override void Update() {
		base.Update();

		if(alreadyInteracted == true) {
			alarmLight.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
		}
	}
}

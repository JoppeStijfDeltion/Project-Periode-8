using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*An object with different types of music, this game its main audiosource*/

public class Radio : InteractableObject {

	[Header("Radio Settings:")]
	public bool isOn = false; //This function defines if the radio can send or not;
	public AudioClip secret;

	#region Private Variables
	private int channelIndex = 0;

	[HideInInspector]
	public AudioSource aSource;
	#endregion

	public override void Awake() {
		base.Awake();
		aSource = GetComponent<AudioSource>();
	}

	public override void Interact() {
		if(channelIndex < sounds.Count - 1) { //If the channel index exists and is smaller than the length;
			channelIndex++;
		}  else {
			channelIndex = 0;
		}

		aSource.clip = sounds[channelIndex];
		aSource.Play();

		hand = null;
	}
}

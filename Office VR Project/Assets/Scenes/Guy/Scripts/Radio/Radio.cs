using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*An object with different types of music, this game its main audiosource*/

public class Radio : RayInteraction {

	[Header("Radio Settings:")]
	public bool isOn = false; //This function defines if the radio can send or not;
	public AudioClip[] channels;

	#region Private Variables
	private int channelIndex = 0;
	#endregion

	public override void Activate() {
		if(isOn == true) { //If the radio is on;
			isOn = false; //Deactivate the radio;
		} else //Else if it is the other way around;
		{
			isOn = true; //Turn the radio on instead;
		}

		TogglePower();
	}

	private void TogglePower() { //Used to turn this radio on or off;
		switch(isOn) { //Switching the bool to check if the radio is on or not;
			case true: //If its on, turn it off;
			aSource.Play();
			break;

			case false: //If its off, turn it on;
			aSource.Pause();
			break;
		}
	}

	private void ToggleChannel() {
		if(channelIndex < channels.Length - 1) { //If the channel index exists and is smaller than the length;
			channelIndex++;
			aSource.clip = channels[channelIndex];
		} 
	}
}

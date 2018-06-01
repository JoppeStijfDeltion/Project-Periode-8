using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*An object with different types of music, this game its main audiosource*/

public class Radio : RayInteraction {

	[Header("Radio Settings:")]
	public bool isOn = false; //This function defines if the radio can send or not;

	public enum RadioChannel {Sender_One, Sender_Two, Sender_Three, Sender_Four, Sender_Five, Sender_Six, Secret}
	public RadioChannel radioState;

	public override void Activate() {
		switch(isOn) { //Switching the bool to check if the radio is on or not;
			case true: //If its on,
			break;

			case false:
			break;
		}
	}
}

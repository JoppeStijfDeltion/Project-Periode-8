using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*These are keys that can be directly connected to the input of a keypad*/

public class Key : RayInteraction {

	[Header("Key Settings:")]
	public char indentifier; //The input that this key contains;
	public bool isEnter = false; //If its an enter, this key will be used to confirm the input;

	[Header("Connection Settings:")]
	public Keypad keypad;
	public Computer computer;
	public enum ConnectedDevice {Keypad, Computer}
	public ConnectedDevice state;

	public override void Activate() { 
		//anim.SetTrigger("Press"); //Plays animation of the button being pressed
		AudioManager.audioManager.PlayAudio(sounds[0], transform); //Plays audio that sounds like something is pressed;

		switch(isEnter) {
			case true:
			if(state == ConnectedDevice.Keypad) {//If the connected device is a keypad;
			if(keypad.completed == false) //If keypad hasn't been completed yet;
			keypad.Unlock(); //Lets the keypad check if the input is correct;
			} else if(state == ConnectedDevice.Computer) {
				computer.CheckForValidCommand(); //Checks the computer if the input has a valid command;
			}
			break;

			case false:
			if(state == ConnectedDevice.Keypad) { //If the connected device is a keypad;
			if(keypad.completed == false) //If keypad hasn't been completed yet;
			keypad.AddChar(indentifier); //Contributes to the keypad its input;
			} else if(state == ConnectedDevice.Computer) {
				computer.AddChar(indentifier);
			}
			break;
			}
		}
	}

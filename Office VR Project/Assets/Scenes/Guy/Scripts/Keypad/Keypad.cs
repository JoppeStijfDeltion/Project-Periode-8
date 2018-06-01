using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Used to input codes to unlock certain mechanisms*/

[RequireComponent(typeof(AudioSource))]
public class Keypad : MonoBehaviour {

	[Header("Keypad Settings:")]
	public string code; //The code required to complete this;
	public bool completed = false; //If this keypad is completed or not;

	[Header("Input Settings:")]
	public Text input; //Your current input;
	public int maxChars; //Max amount of chars that can reside within the input;

	[Header("Unlock Settings:")]
	public Safedoor door; //The door that will be unlocked upon completion;

	[Header("Sound Settings:")]
	public AudioClip[] sounds;

	#region Private Variables
	private bool wrongCode = false;
	private AudioSource aSource;
	#endregion
	
	bool CharCheck() { //Checks if the input is not too long;
		if(input.text.Length <= maxChars)
			return true;

			return false;
	}

	public void Awake() {
		aSource = GetComponent<AudioSource>();
	}

	public void AddChar(char _Input) { //Adds to the input of the keypad;
		if(completed == false) {
		bool canAdd = CharCheck();

		ResetInput();

		if(canAdd) 
			input.text += _Input;
		}
	}

	private void ResetInput() {
		if(wrongCode == true) {
		input.text = "";
		wrongCode = false;
		}
	}

	public void Unlock() { //If input matches the code, unlocks whatever mechanism is selected;
		if(input.text == code) {
			completed = true;
			door.OpenDoor(); //Unlocks door and completed the keypad;
			input.text = "Passed";
			input.color = Color.green;
			AudioManager.audioManager.PlayAudio(aSource, sounds[1]);
		} else {
			input.text = "Wrong"; //Resets input;
			wrongCode = true;
			AudioManager.audioManager.PlayAudio(aSource, sounds[0]);
		}
	}
}

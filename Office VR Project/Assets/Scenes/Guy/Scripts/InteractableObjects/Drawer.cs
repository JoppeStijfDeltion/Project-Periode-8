using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This object is only affected by one axis based on the object its grasped onto */

public class Drawer : InteractableObject {

	public bool open = true;

	[Header("Unlock Settings:")]
	public GameObject containedObject; //Object that will instantiate once the drawer is unlocked;
	public Transform instancePos; //Transform where the content will be instantiated upon unlock;

	[Header("Drawer Settings:")]
	[Tooltip("The max range from the startoffset the drawer can open at.")]
	public float maxOpenOffset = 0.8f;
	[Tooltip("For correcting the distance between the hand and the knob.")]
	public float pivotOffet = 2;
	[Tooltip("Smoothing variable.")]
	public float drawSpeed = 1;

	#region Private Variables
	/*Drawer Related*/
	private Vector3 currentLocation;
	private float startOffset;
	private bool openedAudio = false;
	#endregion

	public override void UpdateAnimations(){}

	public override void Awake() {
		base.Awake();
		currentLocation = transform.position; //Correcting some transforms;
		startOffset = transform.position.x;
	}

	public override void Update() {
		base.Update();
		Audio();
		transform.position = currentLocation;

	}

	private void Audio() {
		if(hand != null) {
			if(openedAudio == false) {
				openedAudio = true;
				AudioManager.audioManager.PlayAudio(sounds[0], transform); //If the drawer is starting to get grabbed on, play audio;
			}
		} else {
			openedAudio = false;
		}
	}

	public void Unlock() {
		if(open == false) { //If the drawer hasn't been unlocked yet;
		open = true; //Set the state to being unlocked;
		GameObject _Contained = Instantiate(containedObject, instancePos.transform.position, Quaternion.identity); //Spawn the contained item;
		_Contained.transform.SetParent(gameObject.transform);
		if(!_Contained.GetComponent<Friction>()) { //If the object doesn't have friction;
		_Contained.AddComponent<Friction>(); //Add friction component;
		_Contained.GetComponent<Friction>().distanceBelowTillFriction = 0.5f;
		}
		//AudioManager.audioManager.PlayAudio(aSource, sounds[1]); //Play unlocking audio cueue;
		}
	}

	public override void Interact() {
		if(hand == null || open == false) { openedAudio = false; return;	} //If there is no object to track, function will be cut off;
			
				currentLocation.x = Mathf.Lerp(currentLocation.x, hand.transform.position.x + pivotOffet, drawSpeed * Time.deltaTime);
				currentLocation.x = Mathf.Clamp(currentLocation.x, startOffset, startOffset + maxOpenOffset);

		}
	}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This object is only affected by one axis based on the object its grasped onto */

public class Drawer : InteractableObject {

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
	private bool opened = false;
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

		GetComponent<MeshRenderer>()
	}

	private void Audio() {
		if(hand != null) {
			if(opened == false) {
				opened = true;
				AudioManager.audioManager.PlayAudio(aSource, sounds[0]); //If the drawer is starting to get grabbed on, play audio;
			}
		} else {
			opened = false;
		}
	}

	public override void Interact() {
		if(hand == null) { opened = false; return;	} //If there is no object to track, function will be cut off;
			
				currentLocation.x = Mathf.Lerp(currentLocation.x, hand.transform.position.x + pivotOffet, drawSpeed * Time.deltaTime);
				currentLocation.x = Mathf.Clamp(currentLocation.x, startOffset, startOffset + maxOpenOffset);

		}
	}

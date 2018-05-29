using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Objects with these scripts tend to have a different function than being picked up */

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public abstract class InteractableObject : AllInteractables {

	[Header("Audio:")]
	public List<AudioClip> sounds; //Audioclips;

	[Header("Current Status:")]
	public PickupSystem hand; //Interacted object;

	#region Private Variables
	protected AudioSource aSource; //Audiosource;
	protected Animator anim; //Animator;
	#endregion

	public virtual void Awake() { //Sets references;
		aSource = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
	}

	public virtual void Update() {
		UpdateAnimations();

		if(hand != null)
		{
		Interact();
		}
	}

	public virtual void UpdateAnimations() {}

	public abstract void Interact(); //Main function;

}

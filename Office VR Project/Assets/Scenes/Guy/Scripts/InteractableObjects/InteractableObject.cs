using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Objects with these scripts tend to have a different function than being picked up */

[RequireComponent(typeof(Animator))]
public abstract class InteractableObject : MonoBehaviour {

	[Header("Audio:")]
	public List<AudioClip> sounds; //Audioclips;

	[Header("Current Status:")]
	public PickupSystem hand; //Interacted object;

	#region Private Variables
	protected Animator anim; //Animator;
	#endregion

	public virtual void Awake() { //Sets references;
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

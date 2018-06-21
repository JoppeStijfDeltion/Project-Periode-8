using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*An object that can be rotated once some conditions are met*/

public class Door : InteractableObject {

	[Header("Door Options")]
	public GameObject hinge;
	public float minRotation;
	public float maxRotation;
	public float followingspeed = 2;
	public bool locked = false;
	public bool calculateNegatives = false;

	#region Private Variables
	private bool selected = false;
	private bool doorOpened = false; //If the opening audio has played;

	private Vector3 currentRotation;
	#endregion

	public override void Awake() {
		base.Awake();
		anim = GetComponent<Animator>(); //Sets reference to the animator;
		currentRotation = hinge.transform.localEulerAngles; //Sets Vector3 to the rotation of this object;
		anim.StopPlayback();
	}

	public override void Update() {
		base.Update(); //Gets information of the parent class;
		Audio();
		hinge.transform.localEulerAngles = currentRotation; //Sets the rotation to a local Vector3 for modifying purposes;
	}

	public override void UpdateAnimations() { //Function solely based on animation updating;
		anim.SetBool("Selected", selected); //A bool to check if this object has been selected or not for animation purposes;

		if(hand == null)
		selected = false;
		else
		selected = true;
	}

	public void Audio() {
		if(hand != null) {
			if(doorOpened == false) {
			AudioManager.audioManager.PlayAudio(sounds[1], transform);
			doorOpened = true;
			} 
		} else {
			doorOpened = false;
		}
	}

	public void Unlock() {
		locked = false;
		AudioManager.audioManager.PlayAudio(sounds[0], transform);
	}

	public override void Interact() {
			if(locked == false) { //If door is off the lock;
			Quaternion targetRot = Quaternion.LookRotation(hand.transform.position - hinge.transform.position); 
			currentRotation.y = Mathf.Lerp(currentRotation.y, ClampAngle(targetRot.eulerAngles.y), followingspeed * Time.deltaTime); //Smoothing the interpolation to open;
			return;
			}
		}

	private float ClampAngle(float angle) { //Clamp function based on the bug when eulerangles go negative;
	if(calculateNegatives == true) {
	  if(angle > 180)
		angle = 360 - angle;
	  	angle = Mathf.Clamp(angle, minRotation, maxRotation);
 	 if(angle < 0) 
	  	angle = 360 + angle;
	  return angle;
	}


	 if(angle < 0) 
		 angle = 360 + angle;

     if (angle < minRotation)
         angle = minRotation;
    else if (angle > maxRotation)
        angle = maxRotation;
     

     return angle;
	}
}

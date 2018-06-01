using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Safedoor : MonoBehaviour {

	public int locks = 3;

	#region Private Variables
	private bool opened = false;
	private Animator anim;
	#endregion

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	public void Update() {
		if(Input.GetKeyDown("o"))
		OpenDoor();
		if(Input.GetKeyDown("p"))
		CloseDoor();
	}

	public void OpenDoor() {
		locks--;

		if(locks <= 0 && opened == false) { //If there aren't any locks left, open up the door;
			opened = true;
			anim.SetTrigger("Open");
		}
	}

	public void CloseDoor() {
		anim.SetTrigger("Close");
	}
}

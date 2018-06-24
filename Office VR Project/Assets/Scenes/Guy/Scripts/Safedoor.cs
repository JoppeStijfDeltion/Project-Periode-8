using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Safedoor : MonoBehaviour {

	public int locks = 3;

	#region Private Variables
	[HideInInspector]
	public bool opened = false;

	[HideInInspector]
	public Animator anim;
	#endregion

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	public void OpenDoor() {
		locks--;

		if(locks <= 0 && opened == false) { //If there aren't any locks left, open up the door;
			opened = true;
			anim.SetTrigger("Open");
			RegionManager.regionManager.LoadRegion(1);
		}
	}

	public void CloseDoor() {
		anim.SetTrigger("Close");
	}
}

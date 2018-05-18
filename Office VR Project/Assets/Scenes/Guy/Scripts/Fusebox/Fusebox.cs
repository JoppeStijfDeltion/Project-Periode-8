using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusebox : MonoBehaviour {

	public static Fusebox fuseBox;

	[Header("Fusebox Settings:")]
	public Fuse[] fuses;
	public Door door;
	public bool completed = false;

	private void Awake() {
		if(fuseBox == null)
		fuseBox = this;
		else if(fuseBox != null)
		Destroy(this);
		
	}

	public void Completion() {
		foreach(Fuse fuse in fuses) {
			if(fuse.activated == false)
				return;
		}

		completed = true;
		door.locked = false;
	}
}

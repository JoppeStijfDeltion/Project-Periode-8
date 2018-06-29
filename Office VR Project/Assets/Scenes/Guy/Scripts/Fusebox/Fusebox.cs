using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A puzzle where every fuse must be green lit*/

public class Fusebox : MonoBehaviour {

	public static Fusebox fuseBox;
    public bool started = false;

	[Header("Fusebox Settings:")]
	public List<Fuse> fuses = new List<Fuse>();
	public Safedoor door;
	public bool completed = false;

	[Header("Fuse Settings:")]
	public Material turnedOn, turnedOff;

	[Header("Debug")]
	public bool[] checks;

	private void Awake() {
		if(fuseBox == null)
		fuseBox = this;
		else if(fuseBox != null)
		Destroy(this);
		
	}

	public void Completion() {
		foreach(Fuse f in fuses) {
			if(f.activated == false) {
			return;
			}
		}

		completed = true;
		door.OpenDoor();
	}
}

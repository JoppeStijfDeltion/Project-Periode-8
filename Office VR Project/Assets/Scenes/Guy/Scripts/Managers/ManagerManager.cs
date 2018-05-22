using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerManager : MonoBehaviour {

	public virtual void Awake() { //Used to set up some parameters upon initialization;
		Initialization();
	}

	public abstract void Initialization();
}

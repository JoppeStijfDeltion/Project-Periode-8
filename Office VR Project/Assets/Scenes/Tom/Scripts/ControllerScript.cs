﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

	#region Get Controller (Input)
	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int) trackedObj.index); }
	}

	private void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}
	#endregion

}
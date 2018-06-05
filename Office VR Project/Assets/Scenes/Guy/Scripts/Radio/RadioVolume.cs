﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioVolume : RayInteraction {

	[Header("Radio Settings:")]
	public Radio radio; //The radio influenced by this script;
	[Tooltip("Z rotation is used to set volume.")]
	public Vector3 rotation; //Used to transcribe the volume
	public float volumeIncrease; //The amount of volume increased per interaction of this object;

	protected override void Awake() {
		base.Awake();
		rotation = transform.localEulerAngles; //Sets the predefined parameters;
	}

	public override void Activate() {
		AudioSource _Radio = radio.GetComponent<AudioSource>(); //The audiosource that the radio itself is emitting;
		transform.localEulerAngles = rotation;

		rotation.z += volumeIncrease;

		_Radio.volume = rotation.z / 360;
		rotation.z = Mathf.Clamp(rotation.z, 1, 359);

		if(Mathf.Approximately(rotation.z, 359)) //If the volume reached the maxAmount;
		rotation.z = 1; //Reset the volume
	}
}

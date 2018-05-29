using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This manager its sole purpose is dealing with audio*/

public class AudioManager : ManagerManager {

	public static AudioManager audioManager;

	public override void Initialization() {
		if(audioManager == null) { audioManager = this; return; }
		Destroy(this);
	}

	public void PlayAudio(AudioSource _AudioSource, AudioClip _AudioClip) {
		if(_AudioSource != null) {
			if(_AudioClip != null)
			_AudioSource.clip = _AudioClip;
			_AudioSource.Play();
			return;
		}

		Debug.LogError(_AudioSource.gameObject + " overloaded a invalid audioclip or audiosource: ErrorInfo: Audiosource = " +_AudioSource +" && Audioclip = "+_AudioClip);
	}
}

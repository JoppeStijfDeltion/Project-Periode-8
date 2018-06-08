using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This manager its sole purpose is dealing with audio*/

public class AudioManager : ManagerManager {

	[Header("Prefab Component:")]
	public AudioSource audioSettings;

	[Header("Universal Clips:")]
	public AudioClip collision;

	public static AudioManager audioManager;

	public override void Initialization() {
		if(audioManager == null) { audioManager = this; return; }
		Destroy(this);
	}

	public void PlayAudio(AudioClip _AudioClip, Transform _Pos) {
		if(_AudioClip != null) { //If a audioclip exists;
			GameObject _Sound = new GameObject(_AudioClip.ToString()); //Instantiates the gameobject with the name of the audiofile;
			_Sound.AddComponent<AudioSource>(); //Adds the audiosource component;
			_Sound.transform.position = _Pos.position; //Sets the position of the newely found audiosource;

			AudioSource _SoundComponent = _Sound.GetComponent<AudioSource>(); //Sets the reference to the component;
			_SoundComponent = audioSettings; //Sets the parameters of the audiosource;
			_SoundComponent.enabled = true; //Activates the audio component;
			_SoundComponent.clip = _AudioClip; //Overloads the clip into the audiosource;
			_SoundComponent.Play(); //Plays the audioclip;
			_Sound.AddComponent<Destroy>(); //Adds the destroy component;
			StartCoroutine(_Sound.GetComponent<Destroy>().DestroySelf(_SoundComponent.clip.length));
		}
	}
}

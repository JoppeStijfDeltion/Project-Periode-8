using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class RayInteraction : MonoBehaviour {

	[Header("Audio Settings:")]
	public AudioClip[] sounds;

	#region Private Variables
	protected Animator anim;

	[HideInInspector]
	public AudioSource aSource;
	#endregion

	protected virtual void Awake() { //Sets some references;
		anim = GetComponent<Animator>();
		aSource = GetComponent<AudioSource>();
	}

	public virtual void PlayAudio(int _AudioClip) {
		if(sounds[_AudioClip] != null) { //If audioclip exists it will follow the function;
			aSource.clip = sounds[_AudioClip]; //Puts the audioclip in the audiosource;
			aSource.Play(); //Plays the audioclip;
			return;
			}
		Debug.LogError(gameObject + " its soundfile you selected does not exist."); //If audioclip is a null;s
		}

	public abstract void Activate(); //Main function;
}

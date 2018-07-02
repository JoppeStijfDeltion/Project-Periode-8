using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoor : MonoBehaviour {

	public enum Doorstate {Restored, Flickering, Off}
	public Doorstate state;

	[Header("Emergency Door Settings:")]
	public bool opened = false;
	public GameObject emergencyDoor;
	public GameObject fuseDoor;
	public Light emergencyLight;
	public float power;
	public float FlickerInterval { get { return Random.Range(0.1f, 2f); } }
	public AudioClip shock;
	public AudioClip finalShock;
	public ParticleSystem smoke;

	[Header("Power Settings:")]
	public float startPower = 100;
	public float flickerPower = 50;

	#region Private Variables
	private Animator emergencyAnim;
	private Animator fuseAnim;
	private bool isFlickering = false;
	#endregion

	void Start () {
		emergencyAnim = emergencyDoor.GetComponent<Animator>();
		fuseAnim = fuseDoor.GetComponent<Animator>();
		power = startPower;
		smoke.Stop();
	}

	public void Open() {
		opened = true;
		fuseAnim.SetTrigger("Open");
				emergencyAnim.SetTrigger("Open");
	}
	
	public void IncrementPower(float _PowerDecrease) {
		if(opened == false)
		return;

		power -= _PowerDecrease;

		if(power <= flickerPower) {
		EffectManager.effectManager.InstantiateEffect("Sparks", transform);
		state = Doorstate.Flickering;
		if(isFlickering == false) {
			isFlickering = true;
			StartCoroutine(Flickering());
		}
	}

	    if(power <= 0) {
		state =	Doorstate.Off;
		emergencyAnim.SetTrigger("Open");
		RegionManager.regionManager.LoadRegion(4);
		//End game;
		}
	}

	public IEnumerator Flickering() {
		yield return new WaitForSeconds(FlickerInterval);
		EffectManager.effectManager.InstantiateEffect("Sparks", transform);
		AudioManager.audioManager.PlayAudio(shock, transform);

		if(emergencyLight.enabled == true) 
			emergencyLight.enabled = false;
		else if(emergencyLight.enabled == false)
			emergencyLight.enabled = true;

		if(state == Doorstate.Flickering)
		StartCoroutine(Flickering());
		else if(state == Doorstate.Off) {
			emergencyLight.enabled = false;
			AudioManager.audioManager.PlayAudio(finalShock, transform);
			smoke.Play();
			StopCoroutine(Flickering());
		}
	}
}

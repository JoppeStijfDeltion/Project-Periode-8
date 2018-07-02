using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : InteractableObject {

	public GameObject ui;
	public Text timerFinal;
	public bool activatedButton;
	public bool canPush = true;
	public int timer = 5;

	public override void Interact() {
		if(canPush == true) {
		AudioManager.audioManager.PlayAudio(sounds[0], transform);

		GetComponent<Animator>().SetTrigger("Press");

		if(activatedButton == false) {
		ui.GetComponent<Animator>().SetTrigger("Open");
		activatedButton = true;
		timerFinal.text = GameManager.gameManager.minutes + ":" + GameManager.gameManager.tenSeconds + GameManager.gameManager.seconds;
		} else {
			canPush = false;
			GameManager.gameManager.startedGame = false;
			RegionManager.regionManager.fade = true;
			StartCoroutine(Quit());
			}
		}
		
		hand = null;
	}

	public IEnumerator Quit() {
		yield return new WaitForSeconds(timer);
		Application.Quit();
	}
}

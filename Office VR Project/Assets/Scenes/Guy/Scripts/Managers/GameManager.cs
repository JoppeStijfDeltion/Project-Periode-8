using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ManagerManager {

	public static GameManager gameManager;

	[Header("Selected Settings:")]
	public Material selected;

	[Header("GameManager Settings:")]
	public bool startedGame = false;
	public int minutes = 0;
	public int seconds = 0;
	public Text timeUI;

	#region Private Variables
	private float timer;
	private int tenSeconds; //Amount of times 10 seconds passed;
	#endregion

	public override void Initialization() {
		if(gameManager == null) { gameManager = this; return; } //Sets the static manager;
		Destroy(this); //If its already set, destroy this component;
	}

	public void Update() { //Constant functions;
		TimeHandler(); //Shows the digital clock;
	}

	private void TimeHandler() {
		if(startedGame) {
		IncrementTime();
		SetDigitalClock();
		}
	}

	private void SetDigitalClock() {
		seconds = (int)Mathf.Floor(timer); //Sets up the digital format of seconds;

		if(tenSeconds >= 6) { //Counts up the minutes after every 60 seconds;
			tenSeconds = 0;
			minutes++;
		}

		if(timer >= 10) {
			timer = 0;
			tenSeconds++;
		}

		if(timeUI != null)
		timeUI.text = minutes + ":"+ tenSeconds + seconds;
	}

	private void IncrementTime() {
		timer += Time.deltaTime; //Adds up time to the timer;
	}
}

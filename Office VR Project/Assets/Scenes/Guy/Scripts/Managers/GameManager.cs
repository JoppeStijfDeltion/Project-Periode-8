﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerManager {

	public static GameManager gameManager;

	[Header("GameManager Settings:")]
	public bool startedGame = false;
	public int minutes = 0;
	public int seconds = 0;

	#region Private Variables
	private float timer;
	#endregion

	public override void Initialization() {
		if(gameManager == null) { gameManager = this; return; }
		Destroy(this);
	}

	public void Update() { //Constant functions;
		TimeHandler();
	}

	private void TimeHandler() {
		if(startedGame) {
		IncrementTime();
		SetDigitalClock();
		}
	}

	private void SetDigitalClock() {
		seconds = (int)Mathf.Floor(timer); //Sets up the digital format of seconds;

		if(timer >= 60) { //Counts up the minutes after every 60 seconds;
			minutes++;
			timer = 0;
		}
	}

	private void IncrementTime() {
		timer += Time.deltaTime; //Adds up time to the timer;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This script is used to activate the printer to access an key code*/

public class Computer : RayInteraction {

	[Header("UI Settings:")]
	public GameObject screen;
	public Text input;
	public Text result;
	public int maxChars = 20;
	public float timerTillBlink = 2;

	[Header("DOS Settings:")]
	public float timeTillUpdate;

	[Header("String Container:")]
	public string startCommand;
	public string[] possibleResults;

	#region Private Variables
	private bool blinked = false;
	public bool usingPrinter = false;
	public bool canStoreChars = false;
	private bool hideIndicator = false;
	#endregion

	public override void Activate() { //Function here is used to start up and shut off the pc screen;
		if(screen != null) { //If the computer has a screen selected;
			if(screen.activeSelf == false) //If the screen is off;
			screen.SetActive(true); //Activate the screen;
			else //Else if its not off;
			screen.SetActive(false); //Deactivate the screen;
		}
	}

	public void Update() {
		BlinkingCursor(false); //For visual effects;
	}

	private void BlinkingCursor(bool canSkip) { //Used for a blinking character used for visualization;
		 if (Time.time - timerTillBlink >= 0.5 || canSkip)
       		{
       			timerTillBlink = Time.time;
       			if (blinked == false)
       			{
                    blinked = true;
       				if (input.text.Length < maxChars + 2)
       				{
       					input.text += "_";
       				}
       			}
       			else
       			{
       				blinked = false;
       				if (input.text.Length != 0)
       				{	
						if(input.text.Contains("_"))
       					input.text = input.text.Substring(0, input.text.Length - 1);
       				}
       			}
       		}
	}

	public void AddChar(char _Character) { //Function solely based off adding onto a string;
		if(input.text.Length <= maxChars) { //Checks if the input is within the range of the max amount of chars;
			if(blinked == true) {
				BlinkingCursor(true);
				blinked = false;
				input.text += _Character; //If so, adds it to the input string;
			} else {
				input.text += _Character; //If so, adds it to the input string;
			}
		}
	}

	public void CheckForValidCommand() { //To check if the following really is an command;
		string _Input = input.text;
		string _Command = "";
		canStoreChars = false;

		if(_Input.Contains("-")) { //Checks if is has an indentifier for a command;
			foreach(char _Letter in _Input) { //Goes through every char in this string;
				if(canStoreChars == true) {
				if(_Letter != '_')
				_Command += _Letter;
				}

				if(_Letter == '-') {
					canStoreChars = true;
				}
			}

			IndentifyCommand(_Command); //Overloads commandline;
			return;

		} else { //If no commandtrigger has been found, immidiate null reference;
			NullReference(_Command);
		}
	}

	public void IndentifyCommand(string _Command) {
		input.text = startCommand;
		BlinkingCursor(true);

		if(usingPrinter == true) { //If printer is selected;
			PrinterOptions(_Command);
			return;
		}


		if(_Command.Contains("print")) {
			usingPrinter = true;
			result.text = possibleResults[0];
			return;
		}

		if(_Command.Contains("help")) {
			result.text = "> Available Commands: \n -print \n -debug";
			return;
		}

		NullReference(_Command);
	}

		public void NullReference(string _Input) { //Sets default string when input is not recognized;
			if(_Input != "")
			result.text = ">"+ '[' + _Input+ ']' + " is not recognized as an internal \n or external command, operable program \n or batch file.";
			input.text = startCommand;
			BlinkingCursor(true);
	}


	public void PrinterOptions(string _Command) {
		usingPrinter = false;

		if(_Command.Contains("doorcode")) {
			result.text = "> Added doorcode.jpg to the printing queue";
			return;
		}

		if(_Command.Contains("mike")) {
			result.text = "> Added mike his face to the printing Queue";
			return;
		}

		if(_Command.Contains("meme")) {
			result.text = "> Added something hideous to the printing Queue";
			return;
		}

		NullReference(_Command);
	}
}

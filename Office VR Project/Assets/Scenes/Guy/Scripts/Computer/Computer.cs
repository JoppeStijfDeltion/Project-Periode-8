using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This script is used to activate the printer to access an key code*/

public class Computer : RayInteraction {

	[Header("Secrets:")]
	public Radio radio;

	[Header("UI Settings:")]
	public GameObject screen;
	public Text input;
	public Text result;
	public int maxChars = 20;
	public float timerTillBlink = 2;

	[Header("DOS Settings:")]
	public float timeTillUpdate;

	[Header("Affected Object Settings:")]
	public Printer printer;

	[System.Serializable]
	public struct PrintingOptions {
		public string name;
		public Material print;
	}

	public List<PrintingOptions> possiblePrints = new List<PrintingOptions>();

	[Header("String Container:")]
	public string startCommand;
	public string[] possibleResults;

	[Header("Checks:")]
	public bool usingPrinter = true;
	public bool usingDirectory = false;

	#region Private Variables
	private bool blinked = false;
	private bool canStoreChars = false;
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

	public void IndentifyCommand(string _Command) { //Function is used to determine if a command exists;
		input.text = startCommand;
		BlinkingCursor(true);

		if(usingPrinter == true) { //If printer is selected;
			PrinterOptions(_Command);
			return;
		}

		if(usingDirectory == true) { //If printer is selected;
			DirectoryOptions(_Command);
			return;
		}

		if(_Command.Contains("print")) {
			usingPrinter = true;

			if(printer.isOn == true)
			result.text = possibleResults[0];
			else
			result.text = possibleResults[1];
			return;
		}

		if(_Command.Contains("90smode")) {
			radio.aSource.clip = radio.secret;
			radio.aSource.Play();
			result.text = possibleResults[7];
			return;
		}

		if(_Command.Contains("directory")) {
			usingDirectory = true;
			result.text = possibleResults[2] + "\n > -Desktop"; //This result explains the possible directorys to look at;
			return;
		}

		if(_Command.Contains("help")) {
			result.text = "> Available Commands: \n -print \n -directory";
			return;
		}

		NullReference(_Command);
	}

		private void NullReference(string _Input) { //Sets default string when input is not recognized;
			if(_Input != "")
			result.text = ">"+ '[' + _Input+ ']' + " is not recognized as an internal \n or external command, operable program \n or batch file.";
			input.text = startCommand;
			BlinkingCursor(true);
			}


	private void PrinterOptions(string _Command) { //If the printer command has been selected;
		usingPrinter = false;

		if(!result.text.Contains("-")) //Checks if is has no indentifier for a command;
		result.text = possibleResults[5];

		if(_Command.Contains("turnon")) //If the player has this as input;
		{
		printer.ActivatePrinter(); //The printer will turn on and will have to ability go add to the queuing list;
		result.text = possibleResults[4];
		return;
		}

		if(_Command.Contains("turnoff")) //If the player has this as input;
		{
		printer.DeactivatePrinter(); //The printer will turn on and will have to ability go add to the queuing list;
		result.text = possibleResults[5];
		return;
		}
		
		foreach(PrintingOptions prints in possiblePrints) { //For every possible prints it checks if the command matches one of em;
			if(_Command.Contains(prints.name)) { //If the command matches one of the file types
				if(printer.isOn == true){ //If the printer is turned off;
				printer.AddToQueue(prints.print); //Adds the object to the printing queue;
				result.text = "Added *" +prints.name+".jpg to the queue..."; //Prints that something has been added to the queue;
				return;
			}
		}
	}
		if(printer.isOn == true)
		NullReference(_Command);
		else
		result.text = possibleResults[3];
	}

	private void DirectoryOptions(string _Command) { //Incase the directory has been selected;
		string detectedFiles = "> Following files were found <";
		usingDirectory = false; //Shuts off the reference towards the directory at entering input;

		if(_Command.Contains("desktop")) 
			foreach(PrintingOptions files in possiblePrints) {
				detectedFiles += "\n> "+files.name +".jpg";
			}

			result.text = detectedFiles;
			return;
		}
	}

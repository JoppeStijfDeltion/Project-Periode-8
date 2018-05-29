using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A puzzle where all puzzle pieces must be in the correct spot to proceed*/

public class Puzzlebox : MonoBehaviour {

	public static Puzzlebox puzzleBox;

	[Header("Puzzlebox Settings:")]
	public List<Puzzlespace> allPuzzleSpaces = new List<Puzzlespace>(); //All spaces;

	[Header("Checks:")]
	public bool completed = false; //If this puzzle has been solved;

	private void Awake() { //Setting up the static box;
		if(puzzleBox == null) { //If the static referenced has nothing contained;
			puzzleBox = this; //Then it sets this as the static variable to reference to;
			return; //And cuts off this function immidiately after;
		}

		Destroy(this); //Else it will remove this component;
	}

	public bool CompletionCheck() { //Check if the completion requirements are met;
		foreach(Puzzlespace _Space in allPuzzleSpaces) { //Goes through all puzzlespaces;
			if(_Space.pieceContained != _Space.pieceNeeded) //If the puzzlespace does hold the piece thats needed;
			return false; //Cut off function;

		}

		return true; //IF all empty spaces are filled with their correct counterpart, return true;
	}

	public void Completion() { //Void soley based on checking if the puzzle has been completed;
		completed = CompletionCheck();
		
		if(completed == true) {
			print("You have succesfully finished the puzzle!");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*A puzzle where all puzzle pieces must be in the correct spot to proceed*/

public class Puzzlebox : InteractableObject {

	public static Puzzlebox puzzleBox;

	[Header ("Puzzlebox Settings:")]
	public List<Puzzlespace> allPuzzleSpaces = new List<Puzzlespace> (); //All spaces;
	public Text[] hints; //Used to show where which piece must be located at;
	public float alphaIncrementSpeed = 2;
	public GameObject tools;
	public Animator hingeAnim;


	[Header ("Checks:")]
	public bool completed = false; //If this puzzle has been solved;
	public Color alphaCol;

	#region Private Variables
	public bool decreaseAlpha = false;
	#endregion

	public override void Awake () { //Setting up the static box;
		base.Awake();

		if (puzzleBox == null) { //If the static referenced has nothing contained;
			puzzleBox = this; //Then it sets this as the static variable to reference to;
			return; //And cuts off this function immidiately after;
		}

		Destroy (this); //Else it will remove this component;
	}

	public bool CompletionCheck () { //Check if the completion requirements are met;
		foreach (Puzzlespace _Space in allPuzzleSpaces) { //Goes through all puzzlespaces;
			if (_Space.pieceContained != _Space.pieceNeeded)//If the puzzlespace does hold the piece thats needed;
				return false; //Cut off function;

		}

		return true; //IF all empty spaces are filled with their correct counterpart, return true;
	}

	public void Completion () { //Void soley based on checking if the puzzle has been completed;
		completed = CompletionCheck ();

		if (completed == true) {
			print ("You have succesfully finished the puzzle!");
			tools.SetActive(true);
			StartCoroutine(Open());
		}
	}


	IEnumerator Open() {
		yield return new WaitForSeconds(2);
		hingeAnim.SetTrigger("Open");
	}



	public override void Interact() {
		anim.SetTrigger("Press");
        if (decreaseAlpha)
            decreaseAlpha = false;  else decreaseAlpha = true;
	    	hand = null;
	}

	public override void Update() {
		base.Update();
		ChangeColor();
	}

	void ChangeColor() {
			foreach(Text _Text in hints) {

                if(decreaseAlpha == false) 
				alphaCol.a -= AdjustAlpha();
                else
                alphaCol.a += AdjustAlpha();

                alphaCol.a = Mathf.Clamp01(alphaCol.a);
                _Text.color = alphaCol;
			}
	    }

	float AdjustAlpha() {
		return Mathf.Clamp01(Time.deltaTime * alphaIncrementSpeed);
	}
}
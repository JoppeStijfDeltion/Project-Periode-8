﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzlespace : RayInteraction {

	[Header("Puzzlespace Settings:")]
	public Puzzlepiece pieceNeeded; //The piece this space needs;
	public Puzzlepiece pieceContained; //The piece this space currently holds;
	public Puzzlespace[] influencedSpaces; //Spaces that the object can go to once clicked and it has something;


    private void Start() {
		Puzzlebox.puzzleBox.allPuzzleSpaces.Add(this);
	}

	public override void Activate() {
		MovePiece();
	}

	public void MovePiece() {
		print(gameObject);
		if(pieceContained != null && Puzzlebox.puzzleBox.completed == false) { //If this space is currently holding a piece and if the puzzle box isn't completed yet;
			foreach(Puzzlespace _PuzzleSpaces in influencedSpaces) { //For every puzzle space in the connected spaces around this;
				if(_PuzzleSpaces.pieceContained == null) { //If one of the spaces is free;
					_PuzzleSpaces.pieceContained = pieceContained; //Give the current piece contained to the other space;
					pieceContained.transform.SetParent(_PuzzleSpaces.transform); //Childs the piece to the newely found space;
					pieceContained.transform.localPosition = Vector3.zero; //Gives the piece the same position as the newely found space;
					pieceContained = null; //And removes it from this space;
					Puzzlebox.puzzleBox.Completion(); //Checks if the puzzlebox has been completed;
				}
			}
		}
	}
}
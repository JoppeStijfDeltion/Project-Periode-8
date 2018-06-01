using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This subclass is part of the puzzle pieces that you can move by ray interaction; */

public class Puzzlepiece : RayInteraction {

        [Header("Parent Settings:")]
        public Puzzlespace parent;

        private void Start() { 
            foreach(Puzzlespace _Space in Puzzlebox.puzzleBox.allPuzzleSpaces) {
                if(_Space.pieceContained != null) {
                    if(_Space.pieceContained == this) {
                    parent = _Space;
                    return;
                    }
                }
            }
        }

        public override void Activate() {
            if(parent != null)
            parent.MovePiece();
        }
    }

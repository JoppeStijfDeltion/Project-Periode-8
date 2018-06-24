using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This subclass is part of the puzzle pieces that you can move by ray interaction; */

public class Puzzlepiece : RayInteraction {

    [Header ("Parent Settings:")]
    public Puzzlespace parent;

    public override void Activate () {
        if (parent != null)
            parent.MovePiece ();
    }
}
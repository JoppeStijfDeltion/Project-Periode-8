using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : InteractableObject {

	[Header("Print Settings:")]
	public List<GameObject> toBePrinted;
	public float printInterval = 2;

	public override void Interact() 
	{
		
	}
}

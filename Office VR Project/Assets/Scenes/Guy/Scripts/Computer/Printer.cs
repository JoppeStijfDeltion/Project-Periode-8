using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Printer : InteractableObject {

	[Header("Print Settings:")]
	public List<GameObject> toBePrinted; //The queuing list of all documents ready to be printed;
	public Transform printLoc; //Location of where the papers are being instantiated at;
	public float printInterval = 2; //Time between each paper thats being printed;
	public GameObject paper;

	[Header("Checks:")]
	public bool beenInteracted = true; //For when the player pushes the button, i'll keep printing till nothing is queuing;
	public bool isOn = false;

	[Header("Animation Settings:")]
	public float printAnimationTime; //Used to manipulate the animation time of the papers;

	[Header("UI Settings:")]
	public Text queue; //Queing UI element;

	public override void Update() {
		base.Update();
	}

	public override void Awake() {
		base.Awake();
		UpdateUI();
	}

	public void UpdateUI() { //Updates the current status of documents that are queued
		queue.text = "Queuing \n" + toBePrinted.Count.ToString() + " files";
	}

	public void ActivatePrinter() { //Turns on the printer
		isOn = true;
		beenInteracted = false;
		queue.enabled = true;
		AudioManager.audioManager.PlayAudio(sounds[0], transform);
	}

	public void AddToQueue(Material _Print) { //Creates a print one the paper prefab;
		GameObject _Paper = paper; //Sets up the base of the paper;
		beenInteracted = false; //Makes it so that the printer is ready to print;
		paper.GetComponent<MeshRenderer>().material = _Print; //Assigned the ink to the paper;
		toBePrinted.Add(paper); //Adds it to the queuing list;
		UpdateUI(); //Updates the queuing UI;

	}

	public void DeactivatePrinter() { //Turns off the printer;
		isOn = false;
		beenInteracted = false;
		queue.enabled = false;
	}

	public override void Interact() {
		if(beenInteracted == false && isOn)
		PrintCheck();

	}	

	private void PrintCheck() { //Checks if the conditions are met and if the printer has something to print with;
	bool canPrint = false;
	GameObject printedObj = null;

	if(beenInteracted == false)	{
	anim.SetTrigger("Press");
	beenInteracted = true;
	}

		foreach(GameObject obj in toBePrinted) {
			if(obj != null) {
				canPrint = true;
			} else {
				beenInteracted = false;
			}
				break;
			}

		if(canPrint) {
			printedObj = toBePrinted[0];
			toBePrinted.RemoveAt(0);
			StartCoroutine(Print(printedObj));
		}

		hand = null;
	}

	IEnumerator Print(GameObject _Printed) {
		GameObject printed = (GameObject)Instantiate(_Printed, printLoc.position, Quaternion.identity);
		printed.transform.SetParent(printLoc);
		printed.GetComponent<Animator>().SetFloat("Printspeed", printAnimationTime);
		AudioManager.audioManager.PlayAudio(sounds[1], transform);
		yield return new WaitForSeconds(printInterval);	
		UpdateUI();

		if(printed.GetComponent<Animator>()) {
			Destroy(printed.GetComponent<Animator>());
		}

		if(!printed.GetComponent<Rigidbody>()) {
			printed.AddComponent<Rigidbody>();
			printed.GetComponent<Rigidbody>().AddForce(-printed.transform.forward * 4, ForceMode.Impulse);
		}

		PrintCheck();
	}
}

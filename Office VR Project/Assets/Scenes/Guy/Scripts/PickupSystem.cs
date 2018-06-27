using UnityEngine;
using UnityEngine.UI;

/*This script has the primary use of enabling the player to interact with props and different objects*/

[RequireComponent(typeof(Rigidbody))]
public class PickupSystem : MonoBehaviour {

	public enum Interaction {Default, RayInteraction, Teleporting}
	public Interaction interactionState;

	[Header("UI Settings:")]
	public Text currentMethodText;

	[Header("Teleportation Settings:")]
	public ControllerScript teleport; //Teleport Options;

	[Header("Debug Settings")]
	public GameObject objectBeingCarried = null;

	[Tooltip("This is the interval between 2 stored positions to decide the throw force.")]
	public int framesTillPositionStore = 15;
	public float throwforceMultiplier = 2;

	[Header("Ray Interaction Settings:")]
	public float maxRange; //Maxrange to interact with rayObjects;
	public Color rayColour; //Color of the line thats drawn;
	public LineRenderer rayRepresentation; //Rayline;
	public bool usingRayInteraction = false; //If you are using rayIntereaction;

	private GameObject oldSelected;

	#region Private&Hidden Variables
	private SteamVR_TrackedObject trackedObj;
	[HideInInspector]
	public SteamVR_Controller.Device Controller {	get { return SteamVR_Controller.Input((int)trackedObj.index); }}

	private FixedJoint thisJoint; //Current gameobject its fixed joint;

	private Rigidbody thisBody;

	private Vector3 newLocation;
	private Vector3 oldLocation;

	private GameObject currentlyHovering; //An object which the hand can currently grab;

	[HideInInspector]
	public GameObject currentRaySelectedObject;

	private int frameCount;
	#endregion
	
	private void Start() { //Sets some references;
		thisJoint = GetComponent<FixedJoint>(); //Sets the main joint of this object;
		thisBody = GetComponent<Rigidbody>();
		rayRepresentation.gameObject.SetActive(true);

		if(GetComponent<SteamVR_TrackedObject>() && GameManager.gameManager.virtualReality == true)
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void Update() {
		LetGo(); //Used for when you let go of the button; 
		AdjustVelocity(); //Used to define the throwing speed;
		Toggle(); //This function is used to toggle the way to interact with the environment;
		CoreInteraction(); //Used to decide for which functions to activate;
	}

	private void Toggle() { //This method is used to toggle between multiple interaction methods;
		if((GameManager.gameManager.virtualReality == true)? Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad):Input.GetButtonDown("Fire3")) { //Interaction based toggle;
		rayRepresentation.enabled = false;

		switch(interactionState) {

			case Interaction.Default: //If the interaction state is currently none;
			interactionState = Interaction.RayInteraction; //Set it to Rayinteraction with objects;
			UpdateTool(Interaction.RayInteraction);
			break;

			case Interaction.RayInteraction: //If the interaction state is currently related to ray interaction;
			if(teleport != null) {
			interactionState = Interaction.Teleporting; //Set it to Teleporting;
			teleport.enabled = true;
			teleport.LaserActivation(true);
			} else //Else if the teleport function does not exist;
			interactionState = Interaction.Default; //Update to the next tool in the queue;
			UpdateTool(Interaction.Default); //Updates the current state in the UI;
			break;

			case Interaction.Teleporting: //If the interaction state is currently related to teleporting;
			teleport.LaserActivation(false);
			interactionState = Interaction.Default; //Set it to Default (None);
			UpdateTool(Interaction.Default);
			break;
			}
		}
	}

	private void UpdateTool(Interaction _State) { //Used to update string;
		if(currentMethodText != null) { //If a tool has been detected;
			switch(interactionState) {
				case Interaction.Default: //If the default tool has been selected;
				currentMethodText.text = "Default \n \n Mode";
				break;

				case Interaction.Teleporting: //If the default tool has been selected;
				currentMethodText.text = "Teleporting \n \n Mode";
				break;

				case Interaction.RayInteraction: //If the default tool has been selected;
				currentMethodText.text = "Ray Interaction \n \n Mode";
				break;
			}

			return; //After updating text, cut off function;
		}

		Debug.LogWarning("No tool has been selected to update the UI mode on.");
	}

	private void CoreInteraction() { //Used to decide which functions to activate;
		if(interactionState == Interaction.RayInteraction)
		RayInteraction();

		if(interactionState == Interaction.Teleporting) //If you selected the teleportation action, it will be enabled;
			teleport.enabled = true; //Sets teleporting to true;
		else
			teleport.enabled = false; //Sets teleporting to false;
	}

	#region RayInteraction
	public void RayInteraction() { //Used for when the player is using a ray to interact with;
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit rayHit;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        rayRepresentation.SetPosition(0, transform.position); //Sets starting position of the line;
        rayRepresentation.material.color = Color.cyan;

        rayRepresentation.SetPositions(new Vector3[] { transform.position, transform.forward * 100 });

        /*This is the functionality part of the ray interaction function */
        if (rayRepresentation.enabled == false) //Check if the ray is turned off;
		    rayRepresentation.enabled = true; //If so, turns it back on;

		if (Physics.Raycast(ray, out rayHit, maxRange)) { //If it detects collision;
            rayRepresentation.transform.localScale = rayRepresentation.transform.position - rayHit.point;
            if (rayHit.transform.gameObject.GetComponent<RayInteraction>() && rayHit.transform.GetComponent<MeshRenderer>()) { //If the object detected can be interacted with;
					if((GameManager.gameManager.virtualReality == true)? Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger): Input.GetKeyDown("e")) 
						rayHit.transform.gameObject.GetComponent<RayInteraction>().Activate(); //Interacts with the object;

					if(currentRaySelectedObject == null) { //If the current object is equal to nothing;
						currentRaySelectedObject = rayHit.transform.gameObject; //Grasp new found object;
						currentRaySelectedObject.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 1); //Set the material to its selected format;

					} else if(currentRaySelectedObject != null) { //If the current interacted ray object storage is already occupied;
						if(currentRaySelectedObject != rayHit.transform.gameObject) { //And if the object is not equal to the selected one;
							currentRaySelectedObject.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 0); //Deselect the old one;
							currentRaySelectedObject = rayHit.transform.gameObject; //Sets the newely interacted object;
							currentRaySelectedObject.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 1); //Selects the new one;			
						}
					}

				return; //Cuts off function;
			} 

					if(currentRaySelectedObject != null) {//If there is a old selected object;
						currentRaySelectedObject.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 0); //Deselects old ray object;
						currentRaySelectedObject = null; //Sets current object thats being interacted with to a null;
					}
				}
			}		
	#endregion

	#region PhysicalInteraction
	private void AdjustVelocity() { //Used to decide the velocity of an object;
		newLocation = transform.position; //Constantly updates the last location;

		if(framesTillPositionStore < 15) //Overwrites old position if enough frames went by;
		framesTillPositionStore++;
		else {
			oldLocation = transform.position; //Stores old location of 15 frames ago;
			framesTillPositionStore = 0; //Resets frames counted;
		}

	}

	private void Pickup(GameObject _Object) {
			if(objectBeingCarried != null) {
			objectBeingCarried.transform.parent = null;	
			}

			objectBeingCarried = _Object; //Sets the overloaded object as the object being carried;	
	
			if(objectBeingCarried.GetComponent<InteractableObject>()) {	
			objectBeingCarried.GetComponent<InteractableObject>().hand = this;
			}

			if(objectBeingCarried.GetComponent<Friction>())
			objectBeingCarried.GetComponent<Friction>().canChild = false;

			if(objectBeingCarried.GetComponent<Rigidbody>()) {
			gameObject.AddComponent<FixedJoint>();
			objectBeingCarried.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			objectBeingCarried.GetComponent<Rigidbody>().useGravity = false;
			thisJoint = GetComponent<FixedJoint>();
			objectBeingCarried.transform.SetParent(gameObject.transform); //Childs newfound object to the hand;
			thisJoint.connectedBody = objectBeingCarried.GetComponent<Rigidbody>(); //Connects the rigidbodys between the parent and the child;
		}
	}

	public void Throwing() { //Applying velocity and let go of grip of the object;
		if(objectBeingCarried == null) { return; }
					objectBeingCarried.GetComponent<Rigidbody>().useGravity = true;
					objectBeingCarried.transform.parent = null; //Unchilds it from the hand;
					objectBeingCarried.GetComponent<Friction>().canChild = true;
					objectBeingCarried.GetComponent<Rigidbody>().velocity = (newLocation - oldLocation) * throwforceMultiplier; //Formula to decide velocity;
					if(thisJoint != null) //If the hand has a fixedjoint assigned
					thisJoint.connectedBody = null; //Resets connected rigidbody;
					objectBeingCarried = null; //Resets object;		
					Destroy(thisJoint);				
		}

	public void LetGo() {
		if(objectBeingCarried == null) { return;} //If there is no object being interacted with, cut function off;
		if((GameManager.gameManager.virtualReality == true)? Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger): Input.GetKeyUp(KeyCode.E)) {

			if(objectBeingCarried.GetComponent<InteractableObject>()) {
				objectBeingCarried.GetComponent<InteractableObject>().hand = null; //If its an interactable, stop interaction;
				objectBeingCarried = null;
				return;
			}

			if(objectBeingCarried.GetComponent<Rigidbody>())
				Throwing(); //If it has a rigidbody, throw it away;
		}
	}

	private void Selected(GameObject _SelectedObj, bool _Deselect) { //Used for visual effects;
		if(_SelectedObj == currentRaySelectedObject) { //If the object selected is the same object as previously;
			return; //Cut off function and do not update visuals;
		}

		if(currentlyHovering == null) { //If the currently hovered object is empty;
			currentlyHovering = _SelectedObj; //Fill in the hovered object;

		} else if(currentlyHovering != null) { //Else if the currently hovering object 

			if(_Deselect == true) {
			currentlyHovering.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 0); //Set the material to its selected format;
			currentlyHovering = null; //Change it to the newely hovered object;
			return;
			}

			if(_Deselect == false) {
			currentlyHovering.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 0); //Set the material to its selected format;
			currentlyHovering = _SelectedObj; //Change it to the newely hovered object;
			}
		}

		if(currentlyHovering != null && currentlyHovering.GetComponent<MeshRenderer>() && _Deselect == false) //If the overloaded object is not null;
			currentlyHovering.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 1); //Set the material to its selected format;
	}

		public void OnTriggerEnter(Collider c) {
			if(c.GetComponent<InteractableObject>() && objectBeingCarried == null)
				Selected(c.gameObject, false);
		}

		private void OnTriggerStay(Collider c) {
			bool canPickup = PickupCheck(c.gameObject); //A check to see if the item in range is indeed something to interact with;

			if(canPickup) //If you can pickup the item;
			if((GameManager.gameManager.virtualReality == true)?  Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger): Input.GetKeyDown(KeyCode.E)) //Checks if you are holding down the button;
			Pickup(c.gameObject); //Call Pickup;

		}

		private void OnTriggerExit(Collider c) {
			if(c.GetComponent<InteractableObject>() && c.GetComponent<MeshRenderer>())
			Selected(c.gameObject, true);
		}

		bool PickupCheck(GameObject _Object) {
			if(objectBeingCarried == null)
					if(!_Object.GetComponent<PickupSystem>()) { //If the opposing gameobject has a rigidbody and a friction module;
						if(_Object.GetComponent<Rigidbody>()) { //If the rigidbody is NOT kinematic;
						if(!_Object.GetComponent<Rigidbody>().isKinematic == false)
						return false;
						}

						if(_Object.GetComponent<Friction>() || _Object.GetComponent<InteractableObject>())
						return true;
					}

			//Else returns false;
			return false;
		}
		#endregion
	}

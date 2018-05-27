using UnityEngine;

/*This script has the primary use of enabling the player to interact with props and different objects*/

[RequireComponent(typeof(Rigidbody))]
public class PickupSystem : MonoBehaviour {

	public enum Interaction {Default, RayInteraction, Teleporting}
	public Interaction interactionState;

	[Header("Teleportation Settings:")]
	public Laser teleport; //Teleport Options;

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

	#region Private&Hidden Variables
	/*private SteamVR_TrackedObject trackedObj;
	[HideInInspector]
	public SteamVR_Controller.Device controller {	get { return SteamVR_Controller.Input((int)trackedObj.index); }}*/

	private FixedJoint thisJoint; //Current gameobject its fixed joint;

	private Rigidbody thisBody;

	private Vector3 newLocation;
	private Vector3 oldLocation;

	private int frameCount;
	#endregion
	
	private void Awake() { //Sets some references;
		thisJoint = GetComponent<FixedJoint>(); //Sets the main joint of this object;
		thisBody = GetComponent<Rigidbody>();
		rayRepresentation.gameObject.SetActive(true);

		/*if(GetComponent<SteamVR_TrackedObject>())
		trackedObj = GetComponent<SteamVR_TrackedObject>();*/
	}

	private void Update() {
		LetGo(); //Used for when you let go of the button; 
		AdjustVelocity(); //Used to define the throwing speed;
		Toggle(); //This function is used to toggle the way to interact with the environment;
		CoreInteraction(); //Used to decide for which functions to activate;
	}

	private void Toggle() { //This method is used to toggle between multiple interaction methods;
		if(Input.GetButtonDown("Fire3")  /*|| controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)*/) { //Interaction based toggle;
		rayRepresentation.enabled = false;

		switch(interactionState) {

			case Interaction.Default: //If the interaction state is currently none;
			interactionState = Interaction.RayInteraction; //Set it to Rayinteraction with objects;
			break;

			case Interaction.RayInteraction: //If the interaction state is currently related to ray interaction;
			interactionState = Interaction.Teleporting; //Set it to Teleporting;
			break;

			case Interaction.Teleporting: //If the interaction state is currently related to teleporting;
			interactionState = Interaction.Default; //Set it to Default (None);
			break;
			}
		}
	}

	private void CoreInteraction() { //Used to decide which functions to activate;
		if(interactionState == Interaction.RayInteraction)
		RayInteraction();

		//if(interactionState == Interaction.Teleporting) //If you selected the teleportation action, it will be enabled;
		//teleport.enabled = true; //Sets teleporting to true;
		//else
		//teleport.enabled = false; //Sets teleporting to false;
	}

	#region RayInteraction
	public void RayInteraction() { //Used for when the player is using a ray to interact with;
		Ray ray;
		RaycastHit rayHit;

		/*This is the functionality part of the ray interaction function */
		if(rayRepresentation.enabled == false) //Check if the ray is turned off;
		rayRepresentation.enabled = true; //If so, turns it back on;

		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rayHit, maxRange)) { //If it detects collision;
			rayRepresentation.transform.localScale = new Vector3 (rayRepresentation.transform.lossyScale.x, rayRepresentation.transform.localEulerAngles.y, rayHit.distance * 7); //To draw the ray;
			if(rayHit.transform.gameObject.GetComponent<RayInteraction>()) { //If the object detected can be interacted with;
				if(Input.GetKeyDown("e") /*  ||  controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) */) {
					rayHit.transform.gameObject.GetComponent<RayInteraction>().Activate(); //Interacts with the object;
					}
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
		if(Input.GetKeyDown(KeyCode.E) /* || controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) */) //Checks if you are holding down the button;
		{
			objectBeingCarried = _Object; //Sets the overloaded object as the object being carried;	

			if(objectBeingCarried.GetComponent<InteractableObject>()) {	
			objectBeingCarried.GetComponent<InteractableObject>().hand = this;
			}

			if(objectBeingCarried.GetComponent<Rigidbody>()) {
			gameObject.AddComponent<FixedJoint>();
			thisJoint = GetComponent<FixedJoint>();
			objectBeingCarried.transform.SetParent(gameObject.transform); //Childs newfound object to the hand;
			thisJoint.connectedBody = objectBeingCarried.GetComponent<Rigidbody>(); //Connects the rigidbodys between the parent and the child;
			}
		}
	}

	private void Throwing() { //Applying velocity and let go of grip of the object;
					objectBeingCarried.transform.parent = null; //Unchilds it from the hand;
					objectBeingCarried.GetComponent<Rigidbody>().velocity = (newLocation - oldLocation) * throwforceMultiplier; //Formula to decide velocity;
					if(thisJoint != null) //If the hand has a fixedjoint assigned
					thisJoint.connectedBody = null; //Resets connected rigidbody;
					objectBeingCarried = null; //Resets object;		
					Destroy(thisJoint);				
		}

	public void LetGo() {
		if(objectBeingCarried == null) { return;} //If there is no object being interacted with, cut function off;

		if(Input.GetKeyUp(KeyCode.E) /* || controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)*/) {

			if(objectBeingCarried.GetComponent<InteractableObject>())
				objectBeingCarried.GetComponent<InteractableObject>().hand = null; //If its an interactable, stop interaction;

			if(objectBeingCarried.GetComponent<Rigidbody>())
				Throwing(); //If it has a rigidbody, throw it away;

				objectBeingCarried = null;
		}
	}

		private void OnTriggerStay(Collider c) {
			bool canPickup = PickupCheck(c.gameObject);

			if(canPickup)
			Pickup(c.gameObject);	

		}

		bool PickupCheck(GameObject _Object) {
			if(objectBeingCarried == null)
				if((_Object.GetComponent<Rigidbody>() || _Object.GetComponent<InteractableObject>())) //Checks if the object can be picked up;
					if(!_Object.GetComponent<PickupSystem>()) //Checks if the object is NOT a 
					return true;

			//Else returns false;
			return false;
		}
		#endregion
	}

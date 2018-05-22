using UnityEngine;

/*This script has the primary use of enabling the player to interact with props and different objects*/

<<<<<<< HEAD
[RequireComponent(typeof(Rigidbody))]
public class PickupSystem : MonoBehaviour {

	[Header("Debug Settings")]
	public GameObject objectBeingCarried = null;
=======
[RequireComponent (typeof (Rigidbody), typeof (FixedJoint))]
public class PickupSystem : MonoBehaviour {

	[Header ("Debug Settings")]
	public Rigidbody objectBeingCarried = null;
>>>>>>> 47bfa7809b6ea9a6d98be8bdce64e29c87284be3

	[Tooltip ("This is the interval between 2 stored positions to decide the throw force.")]
	public int framesTillPositionStore = 15;
	public float throwforceMultiplier = 2;

	[Header("Ray Interaction Settings:")]
	public float maxRange;
	public Color rayColour;
	public LineRenderer rayRepresentation;
	public bool usingRayInteraction = false;

	#region Private Variables
	private FixedJoint thisJoint; //Current gameobject its fixed joint;

	private Rigidbody thisBody;

	private Vector3 newLocation;
	private Vector3 oldLocation;

	private int frameCount;
	#endregion

	public void Awake () { //Sets some references;
		thisJoint = GetComponent<FixedJoint> (); //Sets the main joint of this object;
		thisBody = GetComponent<Rigidbody> ();
	}

<<<<<<< HEAD
	private void FixedUpdate() {
		LetGo();

	}

	private void Update() {
		RayInteraction();	
		AdjustVelocity();
	}

	#region RayInteraction
	public void RayInteraction() {
		Ray ray;
		RaycastHit rayHit;

		if(Input.GetButtonDown("Fire3")) //Ray toggle;
		switch(usingRayInteraction) {
			case true:
			rayRepresentation.enabled = false;
			usingRayInteraction = false;
			break;

			case false:
			rayRepresentation.enabled = true;
			usingRayInteraction = true;
			break;
		}

		/*This is the functionality part of the ray interaction function */
		if(usingRayInteraction == true) {
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rayHit, maxRange)) { //If it detects collision;
			print(rayHit.transform.gameObject);
			if(rayHit.transform.gameObject.GetComponent<RayInteraction>()) {
				if(Input.GetButtonDown("Fire2")) {
					rayHit.transform.gameObject.GetComponent<RayInteraction>().Activate();
					return;
					}
				}
			}
		}
	}
	#endregion

	#region PhysicalInteraction
	private void AdjustVelocity() { //Used to decide the velocity of an object;
=======
	private void FixedUpdate () {
		StorePosition ();
	}

	private void StorePosition () { //Used to decide the velocity of an object;
>>>>>>> 47bfa7809b6ea9a6d98be8bdce64e29c87284be3
		newLocation = transform.position; //Constantly updates the last location;

		if (framesTillPositionStore < 15) //Overwrites old position if enough frames went by;
			framesTillPositionStore++;
		else {
			oldLocation = transform.position; //Stores old location of 15 frames ago;
			framesTillPositionStore = 0; //Resets frames counted;
		}

	}

<<<<<<< HEAD
	private void Pickup(GameObject _Object) {
		if(Input.GetKeyDown(KeyCode.E)) //Checks if you are holding down the button;
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
					thisJoint.connectedBody = null; //Resets connected rigidbody;
					objectBeingCarried = null; //Resets object;		
					Destroy(thisJoint);				
=======
	private void Pickup (Rigidbody _Object) {
		//_Object.GetComponent<MeshRenderer>().materials[1].SetFloat("_Outline", 0.01f);
		if (Input.GetButtonDown ("OpenVR_R_Axis_Trigger_Squeeze")) //Checks if you are holding down the button;
		{
			//_Object.GetComponent<MeshRenderer>().materials[1].SetFloat("_Outline", 0);
			objectBeingCarried = _Object; //Sets the overloaded object as the object being carried;
			objectBeingCarried.transform.SetParent (gameObject.transform); //Childs newfound object to the hand;
			thisJoint.connectedBody = objectBeingCarried; //Connects the rigidbodys between the parent and the child;
		}
	}

	private void Holding () {
		if (objectBeingCarried != null) //Checks if there is something to throw;
		{
			if (Input.GetButtonUp ("Fire1")) //If you let go;
				Throwing (); //Throws;
>>>>>>> 47bfa7809b6ea9a6d98be8bdce64e29c87284be3
		}

<<<<<<< HEAD
	public void LetGo() {
		if(objectBeingCarried == null) { return;} //If there is no object being interacted with, cut function off;

		if(Input.GetKeyUp(KeyCode.E)) {

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
=======
	private void Throwing () { //Applying velocity and let go of grip of the object;
		objectBeingCarried.transform.parent = null; //Unchilds it from the hand;
		objectBeingCarried.GetComponent<Rigidbody> ().velocity = (newLocation - oldLocation) * throwforceMultiplier; //Formula to decide velocity;
		thisJoint.connectedBody = null; //Resets connected rigidbody;
		objectBeingCarried = null; //Resets object;
	}

	#region Physics
	private void OnTriggerStay (Collider c) {
		bool canPickup = PickupCheck (c.gameObject);

		if (canPickup)
			Pickup (c.gameObject.GetComponent<Rigidbody> ());
		Holding ();
	}

	private void OnTriggerExit (Collider c) {
		if (c.gameObject.GetComponent<Rigidbody> ())
			//c.gameObject.GetComponent<MeshRenderer>().materials[1].SetFloat("_Outline", 0);

			if (objectBeingCarried)
				if (c.transform.gameObject == objectBeingCarried.gameObject)
					Throwing ();

	}
	#endregion

	#region Checks
	bool PickupCheck (GameObject _Object) {
		if (_Object.GetComponent<Rigidbody> () && objectBeingCarried == null) //Checks if the object can be picked up;
			return true;

		//Else returns false;
		return false;
	}
	#endregion
}
>>>>>>> 47bfa7809b6ea9a6d98be8bdce64e29c87284be3

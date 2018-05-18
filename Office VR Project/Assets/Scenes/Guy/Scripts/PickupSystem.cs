using UnityEngine;

/*This script has the primary use of enabling the player to interact with props and different objects*/

[RequireComponent (typeof (Rigidbody), typeof (FixedJoint))]
public class PickupSystem : MonoBehaviour {

	[Header ("Debug Settings")]
	public Rigidbody objectBeingCarried = null;

	[Tooltip ("This is the interval between 2 stored positions to decide the throw force.")]
	public int framesTillPositionStore = 15;
	public float throwforceMultiplier = 2;

	#region Private Variables
	private FixedJoint thisJoint; //Current gameobject its fixed joint;

	private Rigidbody thisBody;

	private bool isThrowing = false;
	private bool canCarry = false;

	private Vector3 newLocation;
	private Vector3 oldLocation;

	private int frameCount;
	#endregion

	public void Awake () { //Sets some references;
		thisJoint = GetComponent<FixedJoint> (); //Sets the main joint of this object;
		thisBody = GetComponent<Rigidbody> ();
	}

	private void FixedUpdate () {
		StorePosition ();
	}

	private void StorePosition () { //Used to decide the velocity of an object;
		newLocation = transform.position; //Constantly updates the last location;

		if (framesTillPositionStore < 15) //Overwrites old position if enough frames went by;
			framesTillPositionStore++;
		else {
			oldLocation = transform.position; //Stores old location of 15 frames ago;
			framesTillPositionStore = 0; //Resets frames counted;
		}

	}

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
		}
	}

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
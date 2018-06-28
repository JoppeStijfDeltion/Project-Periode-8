using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script is used to prevent teleporting through objects, and avoid general clipping with color based feedback*/

public class TeleportRoomCheck : MonoBehaviour {

	public bool canTeleport = true;

    public LayerMask teleportMask; //Mask that allows teleporting;
    public Material particle;
    public Material footsteps;

    public Color falseDetected;
    public Color trueDetected;


    public void Update() {
		CustomTriggerArea(); //Custom written OnTrigger station;
	}

	void CustomTriggerArea() {
		Collider[] _CurrentColliders = new Collider[10]; //Storage for all colliders within the triggerfield;
		Physics.OverlapBoxNonAlloc(transform.position, transform.GetComponent<BoxCollider>().size, _CurrentColliders); //Collecting all current data;

		foreach(Collider _Col in _CurrentColliders) { //For every collider that is within the trigger field;
			if(_Col != null) { //If the collider has data;
			    if(_Col.isTrigger == false) { //If the collider is NOT a triggerfield;
                        canTeleport = true; //Stops the user from teleporting;
                        particle.color = trueDetected;
                        footsteps.color = trueDetected;
                    } else {
                    canTeleport = false; //Stops the user from teleporting;
                    particle.color = falseDetected;
                    footsteps.color = falseDetected;
                }
            } else
            {
                canTeleport = true; //Stops the user from teleporting;
                particle.color = trueDetected;
                footsteps.color = trueDetected;
            }
        }
    }
}

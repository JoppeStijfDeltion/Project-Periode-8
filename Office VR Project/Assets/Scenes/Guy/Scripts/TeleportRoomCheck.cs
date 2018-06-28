using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Script is used to prevent teleporting through objects, and avoid general clipping with color based feedback*/

public class TeleportRoomCheck : MonoBehaviour {

	public bool canTeleport = true;

    public LayerMask teleportMask; //Mask that allows teleporting;
    public Material particle;
    public Image footsteps;

    public Color falseDetected;
    public Color trueDetected;


    public void Update() {
		CustomTriggerArea(); //Custom written OnTrigger station;
	}

	void CustomTriggerArea() {
		Collider[] _CurrentColliders = new Collider[5]; //Storage for all colliders within the triggerfield;
		Physics.OverlapBoxNonAlloc(transform.position + transform.GetComponent<BoxCollider>().center, new Vector3(0.2f ,4, 0.2f), _CurrentColliders); //Collecting all current data;

		foreach(Collider _Col in _CurrentColliders) { //For every collider that is within the trigger field;
			if(_Col != null) { //If the collider has data;
                if((teleportMask.value & 1<<_Col.transform.gameObject.layer) != 0) {
                        canTeleport = true; //Stops the user from teleporting;
                        particle.color = trueDetected;
                        footsteps.color = trueDetected;
                } else {
                        print("Current colliders detected within triggerfield: " +_Col.gameObject);
                        canTeleport = false; //Stops the user from teleporting;
                        particle.color = falseDetected;
                        footsteps.color = falseDetected;
                   }
               }
            }
        }
    }

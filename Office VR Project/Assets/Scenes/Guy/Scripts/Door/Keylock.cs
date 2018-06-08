using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keylock : MonoBehaviour {

	[Header("Lock Options:")]
	public GameObject key;
	public Door affectedDoor;
	public Drawer affectedDrawer;
	public bool unlocked = false;
	
	private void OnTriggerEnter(Collider c) {
		if(unlocked == false)
			if(c.gameObject == key)
			{	
				foreach(PickupSystem _Hand in GameManager.gameManager.hands) {
					if(_Hand.objectBeingCarried == key) { //If the object indentifies as the key used to unlock this drawer;
						Destroy(_Hand.GetComponent<FixedJoint>()); //Removes the connection to the hand;
						_Hand.objectBeingCarried = null; //Removes the object from the hand;
					}
				}

				Destroy(c.GetComponent<Rigidbody>());
				Destroy(c.GetComponent<Collider>());
				c.transform.position = transform.position;
				c.transform.eulerAngles = transform.eulerAngles;
				c.transform.SetParent(gameObject.transform);
				unlocked = true;

				if(affectedDoor != null) //If there is a door to unlock;
				affectedDoor.Unlock();

				if(affectedDrawer != null) //If there is a drawer to unlock;
				affectedDrawer.Unlock();
			}
		}
	}

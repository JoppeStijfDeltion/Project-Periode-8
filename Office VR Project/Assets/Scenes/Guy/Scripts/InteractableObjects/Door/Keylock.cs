using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keylock : MonoBehaviour {

	[Header("Lock Options:")]
	public GameObject key;
	public Door affectedDoor;
	public bool unlocked = false;
	
	private void OnTriggerEnter(Collider c) {
		if(unlocked == false)
			if(c.gameObject == key)
			{
				Destroy(key.GetComponent<Rigidbody>());
				Destroy(key.GetComponent<Collider>());
				key.transform.position = transform.position;
				key.transform.eulerAngles = transform.eulerAngles;
				key.transform.SetParent(gameObject.transform);
				unlocked = true;
				affectedDoor.locked = false;
			}
		}
	}

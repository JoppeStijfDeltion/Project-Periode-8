using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script has the primary use of enabling the player to interact with props and different objects*/

public class PickupSystem : MonoBehaviour {

	[Header("Debug Settings")]
	public GameObject holding;
	public LayerMask eniviroment;

	#region Private Variables
	private GameObject currentObjectSelected;
	#endregion
	
	private void Update() {
		ObjectInteraction();
		CorrectPosition();
	}

	private void CorrectPosition() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, eniviroment)) //check if the ray hit something
        {
            transform.position = hit.point;
        }
    }

    private void ObjectInteraction() {
		if(currentObjectSelected != null)
			if(holding == null)

				if(Input.GetButton("Fire1")) {
				
				if(holding == null)
				holding = currentObjectSelected;

				holding.transform.position = transform.position;
				holding.transform.SetParent(gameObject.transform);
				return;
				} 

					if(holding != null)
					{
					holding.transform.parent = null;
					holding = null;
					}
				}


	public void OnTriggerStay(Collider c) {
		if(c.transform.GetComponent<Pickup>()) {
			currentObjectSelected = c.transform.gameObject;
		}
	}

	public void OnTriggerExit(Collider c) {
		if(c.transform.GetComponent<Pickup>()) {
			currentObjectSelected = null;
		}
	}
}

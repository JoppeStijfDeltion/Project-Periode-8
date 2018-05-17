using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Teleport : MonoBehaviour {

	public Transform hand;

	private NavMeshAgent agent;

	private void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			RaycastHit hit;
			if (Physics.Raycast (hand.position, hand.forward, out hit, 10f)) {
				print (hit.point);
				agent.SetDestination (hit.point);
			}
		}
	}
}
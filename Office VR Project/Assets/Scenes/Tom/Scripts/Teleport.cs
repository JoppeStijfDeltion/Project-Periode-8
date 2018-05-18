using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Teleport : MonoBehaviour {

	public Transform hand;
	private NavMeshAgent agent;

	private void Awake () {
		agent = GetComponent<NavMeshAgent> ();

		RenderLine.spawn = hand;
	}

	private void Update () {
		RaycastHit hit;
		if (Input.GetButtonDown ("Fire1")) {
			if (Physics.Raycast (hand.position, hand.forward, out hit, 10f)) {
				agent.SetDestination (hit.point);
			}
		}
	}
}
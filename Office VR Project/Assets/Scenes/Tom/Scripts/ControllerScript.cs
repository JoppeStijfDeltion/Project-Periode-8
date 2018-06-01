using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

	public static bool vr;
	public Transform handLeft;
	public Transform handRight;

	#region Public Variables

	[Header ("The ''Camera (head)'' object ")]
	public Transform headTransform;
	[Header ("Prefab of the laser")]
	public GameObject LaserPrefab;
	[Header ("Color the laser gets when either teleporting or interacting")]
	public Color teleportColor = Color.red;
	public Color interactColor = Color.blue;
	[Header ("Prefab of the reticle shown when teleport laser is active")]
	public GameObject teleportReticlePrefab;
	[Header ("Laser masks")]
	public LayerMask teleportMask;
	public LayerMask interactMask;
	[Header ("Interaction Ray stuff")]
	public Transform rayTargetLeft;
	public Transform rayTargetRight;
	public float interactDistance = 10f;

	#endregion
	#region Get Controllers (Input)

	// Get the the left and right controller and allow for easy reference for input

	private SteamVR_TrackedObject trackedObjLeft;
	private SteamVR_TrackedObject trackedObjRight;

	private SteamVR_Controller.Device controllerLeft {
		get { return SteamVR_Controller.Input ((int)trackedObjLeft.index); }
	}
	private SteamVR_Controller.Device controllerRight {
		get { return SteamVR_Controller.Input ((int)trackedObjRight.index); }
	}

	#endregion
	#region Laser

	private enum LaserMode {
		Teleport,
		Interact
	}

	// Left Controller
	private GameObject laserLeft;
	private LaserMode laserModeLeft;
	// Right Controller
	private GameObject laserRight;
	private LaserMode laserModeRight;

	#endregion
	#region  Teleport

	private GameObject reticle;
	private bool canTeleport;

	#endregion

	// Change this to private bla bla bla
	public PickupSystem interactRef;

	public float test {
		get {
			return 10f;
		}
		set {

		}
	}

	private void Awake () {

		if (vr) {
			trackedObjLeft = GetComponent<SteamVR_ControllerManager> ().left.GetComponent<SteamVR_TrackedObject> ();
			trackedObjRight = GetComponent<SteamVR_ControllerManager> ().right.GetComponent<SteamVR_TrackedObject> ();
		}
	}

	private void Start () {
		rayTargetLeft.position = new Vector3 (0, rayTargetLeft.position.y, interactDistance);
		rayTargetRight.position = new Vector3 (0, rayTargetRight.position.y, interactDistance);

		laserLeft = Instantiate (LaserPrefab);
		laserRight = Instantiate (LaserPrefab);
		reticle = Instantiate (teleportReticlePrefab);
		reticle.GetComponent<Renderer> ().material.color = teleportColor;

		laserLeft.SetActive (false);
		laserRight.SetActive (false);
		reticle.SetActive (false);
	}

	private void Update () {
		if (vr) {
			GetControllerInput (controllerLeft, trackedObjLeft, laserLeft, rayTargetLeft, laserModeLeft);
			GetControllerInput (controllerRight, trackedObjRight, laserRight, rayTargetRight, laserModeRight);
		}
		else {
			GetKeyboardInput (true);
			GetKeyboardInput (false);
		}
	}

	private void GetControllerInput (SteamVR_Controller.Device controller, SteamVR_TrackedObject trackedObj, GameObject laser, Transform rayTarget, LaserMode laserMode) {
		RaycastHit hit;
		Transform trackedObjTransform = trackedObj.transform;
		switch (laserMode) {
			case LaserMode.Teleport:
				if (controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {

					if (Physics.Raycast (trackedObjTransform.position, trackedObjTransform.forward, out hit, 100f, teleportMask)) {
						ShowTeleportLaser (null, laser, trackedObj, hit);
						reticle.SetActive (true);
						reticle.transform.position = hit.point + new Vector3 (0, 0.05f, 0);
						canTeleport = true;
					}
					else {
						laser.SetActive (false);
						reticle.SetActive (false);
					}

					if (controller.GetHairTrigger ()) {
						Teleport (hit);
					}
				}
				else {
					laser.SetActive (false);
					reticle.SetActive (false);
				}
				break;
			case LaserMode.Interact:
				reticle.SetActive (false);
				if (Physics.Raycast (trackedObjTransform.position, trackedObjTransform.forward, out hit, interactDistance)) {
					ShowInteractLaser (null, laser, rayTarget, trackedObj, true, hit);
				}
				else {
					ShowInteractLaser (null, laser, rayTarget, trackedObj, false, new RaycastHit ());
				}
				if (controller.GetHairTrigger ()) {
					interactRef.RayInteraction ();
				}
				break;
		}
	}

	private void GetKeyboardInput (bool left) {
		LaserMode laserMode;
		Transform hand;
		Transform target;
		GameObject laser;

		if (left) {
			if (Input.GetButtonDown ("Jump")) {
				if (laserModeLeft == LaserMode.Teleport) {
					laserModeLeft = LaserMode.Interact;
				}
				else {
					laserModeLeft = LaserMode.Teleport;
				}
			}
			laserMode = laserModeLeft;
			hand = handLeft;
			target = rayTargetLeft;
			laser = laserLeft;
		}
		else {
			if (Input.GetButtonDown ("Jump")) {
				if (laserModeRight == LaserMode.Teleport) {
					laserModeRight = LaserMode.Interact;
				}
				else {
					laserModeRight = LaserMode.Teleport;
				}
			}
			laserMode = laserModeRight;
			hand = handRight;
			target = rayTargetRight;
			laser = laserRight;
		}

		RaycastHit hit;

		switch (laserMode) {
			case LaserMode.Teleport:
				if (Input.GetButton ("Fire2")) {

					if (Physics.Raycast (hand.position, hand.forward, out hit, 100f, teleportMask)) {
						ShowTeleportLaser (hand, laser, null, hit);
						reticle.SetActive (true);
						reticle.transform.position = hit.point + new Vector3 (0, 0.05f, 0);
						canTeleport = true;
					}
					else {
						laser.SetActive (false);
						reticle.SetActive (false);
					}

					if (Input.GetButtonDown ("Fire1")) {
						Teleport (hit);
					}
				}
				else {
					laser.SetActive (false);
					reticle.SetActive (false);
				}
				break;
			case LaserMode.Interact:
				reticle.SetActive (false);
				if (Physics.Raycast (hand.position, hand.forward, out hit, interactDistance)) {
					ShowInteractLaser (hand, laser, rayTargetRight, null, true, hit);
				}
				else {
					ShowInteractLaser (hand, laser, rayTargetRight, null, false, new RaycastHit ());
				}
				if (Input.GetButtonDown ("Fire1")) {
					interactRef.RayInteraction ();
				}
				break;
		}
	}

	private void ShowTeleportLaser (Transform hand, GameObject laser, SteamVR_TrackedObject trackedObj, RaycastHit hit) {
		laser.GetComponent<Renderer> ().material.color = teleportColor;
		laser.SetActive (true);
		Transform laserTransform = laser.transform;
		if (trackedObj == null) {
			laserTransform.position = Vector3.Lerp (hand.position, hit.point, 0.5f);
		}
		else {
			laserTransform.position = Vector3.Lerp (trackedObj.transform.position, hit.point, 0.5f);
		}
		laserTransform.LookAt (hit.point);
		laserTransform.localScale = new Vector3 (laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
	}

	private void ShowInteractLaser (Transform hand, GameObject laser, Transform rayTarget, SteamVR_TrackedObject trackedObj, bool ray, RaycastHit hit) {
		laser.GetComponent<Renderer> ().material.color = interactColor;
		laser.SetActive (true);
		Transform laserTransform = laser.transform;
		Vector3 pos = (ray)? hit.point : rayTarget.position;
		if (trackedObj == null) {
			// PROBLEM?!
			laserTransform.position = Vector3.Lerp (hand.position, pos, 0.5f);
		}
		else {
			laserTransform.position = Vector3.Lerp (trackedObj.transform.position, pos, 0.5f);
		}
		laserTransform.LookAt (pos);
		float dist = (ray)? hit.distance : Vector3.Distance (hand.position, rayTarget.position);
		laserTransform.localScale = new Vector3 (laserTransform.localScale.x, laserTransform.localScale.y, dist);
	}

	private void Teleport (RaycastHit hit) {
		canTeleport = false;
		reticle.SetActive (false);
		Vector3 difference = transform.position - headTransform.position;
		difference.y = 0f;
		transform.position = hit.point + difference;
	}
}
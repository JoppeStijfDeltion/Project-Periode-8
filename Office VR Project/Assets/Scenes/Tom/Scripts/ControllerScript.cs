using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

	public bool vr;

	[Header ("The ''Camera (head)'' object ")]
	public Transform headTransform;
	[Header ("Prefab of the laser")]
	public GameObject LaserPrefab;
	[Header ("Materials the laser gets when either teleporting or interacting")]
	public Material teleportMaterial;
	public Material interactMaterial;
	[Header ("Prefab of the reticle shown when teleport laser is active")]
	public GameObject teleportReticlePrefab;
	[Header ("Teleport mask, can only teleport on objects with those layers")]
	public LayerMask teleportMask;

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

	private void Awake () {
		if (vr) {
			trackedObjLeft = GetComponent<SteamVR_ControllerManager> ().left.GetComponent<SteamVR_TrackedObject> ();
			trackedObjRight = GetComponent<SteamVR_ControllerManager> ().right.GetComponent<SteamVR_TrackedObject> ();
		}
	}

	private void Start () {
		laserLeft = Instantiate (LaserPrefab);
		laserRight = Instantiate (LaserPrefab);
		reticle = Instantiate (teleportReticlePrefab);

		laserLeft.SetActive (false);
		laserRight.SetActive (false);
		reticle.SetActive (false);
	}

	private void Update () {
		if (vr) {
			GetControllerInput (controllerLeft, trackedObjLeft, laserLeft, laserModeLeft);
			GetControllerInput (controllerRight, trackedObjRight, laserRight, laserModeRight);
		}
	}

	private void GetControllerInput (SteamVR_Controller.Device controller, SteamVR_TrackedObject trackedObj, GameObject laser, LaserMode laserMode) {
		RaycastHit hit;
		Transform trackedObjTransform = trackedObj.transform;
		switch (laserMode) {
			case LaserMode.Teleport:
				if (controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {

					if (Physics.Raycast (trackedObjTransform.position, trackedObjTransform.forward, out hit, 100f, teleportMask)) {
						ShowLaser (laser, trackedObj, hit, teleportMaterial);
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
				if (Physics.Raycast (trackedObjTransform.position, trackedObjTransform.forward, out hit, 100f, teleportMask)) {
					ShowLaser (laser, trackedObj, hit, teleportMaterial);
				}
				if (controller.GetHairTrigger ()) {
					print ("Interact!");
					// Interaction here
				}
				break;
		}
	}

	private void ShowLaser (GameObject laser, SteamVR_TrackedObject trackedObj, RaycastHit hit, Material laserMaterial) {
		laser.GetComponent<Renderer> ().material = laserMaterial;
		laser.SetActive (true);
		Transform laserTransform = laser.transform;
		laserTransform.position = Vector3.Lerp (trackedObj.transform.position, hit.point, 0.5f);
		laserTransform.LookAt (hit.point);
		laserTransform.localScale = new Vector3 (laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
	}

	private void Teleport (RaycastHit hit) {
		canTeleport = false;
		reticle.SetActive (false);
		Vector3 difference = transform.position - headTransform.position;
		difference.y = 0f;
		transform.position = hit.point + difference;
	}
}
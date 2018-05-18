using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	#region Teleportation
	public Transform cameraRigTransform;
	public GameObject teleportReticlePrefab;
	private GameObject reticle;
	private Transform teleportReticleTransform;
	public Transform headTransform;
	public Vector3 teleportReticleOffset;
	public LayerMask teleportMask;
	private bool shouldTeleport;

	private void Teleport () {
		shouldTeleport = false;
		reticle.SetActive (false);
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}
	#endregion

	#region Controller
	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input ((int) trackedObj.index); }
	}
	#endregion

	#region Laser
	public GameObject laserPrefab;
	private GameObject laser;
	private Transform laserTransform;
	private Vector3 hitPoint;

	private void ShowLaser (RaycastHit hit) {
		laser.SetActive (true);
		laserTransform.position = Vector3.Lerp (trackedObj.transform.position, hitPoint, .5f);
		laserTransform.LookAt (hitPoint);
		laserTransform.localScale = new Vector3 (laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
	}
	#endregion;

	private void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	private void Start () {
		// Laser
		laser = Instantiate (laserPrefab);
		laserTransform = laser.transform;

		// Teleportation
		reticle = Instantiate (teleportReticlePrefab);
		teleportReticleTransform = reticle.transform;
	}

	private void Update () {
		if (Controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			RaycastHit hit;
			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, teleportMask)) {
				hitPoint = hit.point;
				ShowLaser (hit);
				reticle.SetActive (true);
				teleportReticleTransform.position = hitPoint + teleportReticleOffset;
				shouldTeleport = true;
			}
		}
		else {
			laser.SetActive (false);
			reticle.SetActive (false);
		}
		if (Controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport) {
			Teleport ();
		}
	}
}
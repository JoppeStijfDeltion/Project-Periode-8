using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class ControllerScript : MonoBehaviour {

	public static bool vr;

	#region Public Variables

	[Header ("The ''Camera (head)'' object ")]
	public Transform headTransform;
	[Header ("Prefab of the laser")]
	public LineRenderer laser;
	[Header ("Color the laser gets when either teleporting or interacting")]
	public Color teleportColor = Color.red;
	public Color interactColor = Color.blue;
	public Material laserMaterial;
	[Header ("Prefab of the reticle shown when teleport laser is active")]
	public GameObject teleportReticlePrefab;
	[Header ("Laser masks")]
	public LayerMask teleportMask;
	public float interactDistance = 10f;

	#endregion
	#region Get Controller (Input)

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	#endregion
	#region Laser

	public enum LaserMode {
		Teleport,
		Interact
	}

	private LaserMode laserMode;

	#endregion
	#region  Teleport

	private GameObject reticle;
	private bool canTeleport;

	#endregion

	private void Awake () {
		laser = GetComponent<LineRenderer> ();
		if (vr) {
			trackedObj = GetComponent<SteamVR_TrackedObject> ();
		}
	}

	private void Start () {
		reticle = Instantiate (teleportReticlePrefab);
		reticle.GetComponent<Renderer> ().material.color = teleportColor;
		laser.enabled = false;
		reticle.SetActive (false);
	}

	public void ShowLaser () {
		RaycastHit hit;
		switch (laserMode) {
			case LaserMode.Teleport:
				if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, teleportMask)) {
					laser.enabled = true;
					laserMaterial.color = teleportColor;
					laser.useWorldSpace = true;
					laser.SetPositions (new Vector3[] { transform.position, hit.point });
					reticle.SetActive (true);
					reticle.transform.position = hit.point + new Vector3 (0, 0.05f, 0);
					canTeleport = true;
				}
				else {
					laser.enabled = false;
					reticle.SetActive (false);
					canTeleport = false;
				}

				bool input = (vr)? controller.GetHairTriggerDown (): Input.GetKeyDown ("e");
				if (input && canTeleport) {
					Teleport (hit);
				}
				break;
			case LaserMode.Interact:
				laser.enabled = true;
				laserMaterial.color = interactColor;
				laser.useWorldSpace = false;
				laser.SetPositions (new Vector3[] { Vector3.zero, Vector3.forward * interactDistance });
				break;
		}
	}

	private void Teleport (RaycastHit hit) {
		canTeleport = false;
		reticle.SetActive (false);
		Vector3 difference = headTransform.parent.position - headTransform.position;
		difference.y = 0f;
		headTransform.parent.position = hit.point + difference;
	}

}
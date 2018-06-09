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
	public Material laserMaterial;
	[Header ("Prefab of the reticle shown when teleport laser is active")]
	public GameObject teleportReticlePrefab;
	[Header ("Laser masks")]
	public LayerMask reticleMask;

	#endregion
	#region Get Controller (Input)

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

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

	private void Update( ) {
		ShowLaser();
	}

	public void LaserActivation(bool _State) { //Used to turn on and turn off the laser function;
		if(_State == true) { //If the laser should be activated;
				laser.enabled = true;
				reticle.SetActive (true);
				canTeleport = true;
				laserMaterial.color = teleportColor;
				laser.useWorldSpace = true;
				return;
		} else

		if(_State == false) { //If the laser should be activated;
				laser.enabled = false;
				reticle.SetActive (false);
				canTeleport = false;
				return;
		}
	}

	public void ShowLaser () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, reticleMask)) {
			if(hit.transform.gameObject.layer == 8) {
					LaserActivation(true);
					laser.SetPositions (new Vector3[] { transform.position, hit.point });
					reticle.transform.position = hit.point + new Vector3 (0, 0.05f, 0);
					} 
					else
					LaserActivation(false);
				} else
					LaserActivation(false);

				bool input = (vr)? controller.GetHairTriggerDown (): Input.GetKeyDown ("e");
				if (input && canTeleport) 
					Teleport (hit);
	}

	private void Teleport (RaycastHit hit) {
		if(reticle.GetComponent<TeleportRoomCheck>().canTeleport) { //If the reticle doesn't collide with anything;
		RegionManager.regionManager.alpha.a = 1; //Fade effect per teleport;
		LaserActivation(false);
		Vector3 difference = headTransform.parent.position - headTransform.position;
		difference.y = 0f;
		headTransform.parent.position = hit.point + difference;
		}
	}
}
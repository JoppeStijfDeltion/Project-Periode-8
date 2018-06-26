﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class ControllerScript : MonoBehaviour {

	[Header("Sound:")]
	public AudioClip footsteps;

	[Header("Camera Head Rig Object:")]
	public Transform headTransform;

	[Header ("Ray Reference:")]
	public LineRenderer rayVisual;

    [Header("Prefab of the reticle shown when teleport laser is active")]
    public GameObject teleportMarker;
    public Vector3 teleportScale;

	[Header ("Allow Teleport Mask:")]
	public LayerMask teleportMask;

	#region Get Controller (Input)

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

    #endregion

    #region
    private float height = 0;
#endregion

    #region  Teleport

    private GameObject reticle;
	private bool canTeleport;

	#endregion

	private void Awake () {
		rayVisual = GetComponent<LineRenderer>();
		reticle = Instantiate (teleportMarker);
        reticle.transform.localScale = teleportScale;
        reticle.SetActive(false);
        height = headTransform.position.y;
 
    }

	private void Start () {
		if (GameManager.gameManager.virtualReality == true) {
			trackedObj = GetComponent<SteamVR_TrackedObject> ();
		}

		rayVisual.enabled = false;
		reticle.SetActive (false);
	}

	private void Update( ) {
		ShowLaser();
	}

	public void LaserActivation(bool _State) { //Used to turn on and turn off the laser function;
		if(_State == true) { //If the laser should be activated;
				rayVisual.enabled = true;
				reticle.SetActive (true);
				canTeleport = true;
				rayVisual.useWorldSpace = true;
				return;
		} else

		if(_State == false) { //If the laser should be activated;
				rayVisual.enabled = false;
				reticle.SetActive (false);
				canTeleport = false;
				return;
		}
	}

	public void ShowLaser () {
        if (this.enabled == true)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity)) {
                rayVisual.SetPositions(new Vector3[] { transform.position, hit.point });
                reticle.transform.position = hit.point + new Vector3(0, 0.05f, 0);
                Debug.Log("<color=yellow>" + hit.transform.name + "</color>");

                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, teleportMask))
                {
                    rayVisual.material.color = Color.green;
                    bool input = (GameManager.gameManager.virtualReality == true) ? controller.GetHairTriggerDown() : Input.GetKeyDown("e");
                    if (input && canTeleport)
                        Teleport(hit);
                }
                else
                {
                    reticle.SetActive(false);
                    rayVisual.material.color = Color.red;
                }
            }
        }
	}

	private void Teleport (RaycastHit hit) {
		if(reticle.GetComponent<TeleportRoomCheck>().canTeleport == true) { //If the reticle doesn't collide with anything;
		RegionManager.regionManager.alpha.a = 1; //Fade effect per teleport;
        Vector3 difference = headTransform.parent.position - headTransform.position;
        Vector3 test = headTransform.position;
        test.y = height;
        headTransform.position = test;   
        headTransform.parent.position = hit.point + difference; 
		AudioManager.audioManager.PlayAudio(footsteps, transform);
		}
	}
}
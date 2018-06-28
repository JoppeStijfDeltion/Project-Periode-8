using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ControllerScript : MonoBehaviour
{

    [Header("Sound:")]
    public AudioClip footsteps;

    [Header("Camera Head Rig Object:")]
    public Transform headTransform;

    [Header("Ray Reference:")]
    public LineRenderer rayVisual;

    [Header("Prefab of the reticle shown when teleport laser is active")]
    public GameObject teleportMarker;

    [Header("Allow Teleport Mask:")]
    public LayerMask teleportMask;
    public LayerMask reticleMask;

    #region Get Controller (Input)

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    #endregion

    #region
    private float height = 0;
    #endregion

    #region  Teleport

    private GameObject reticle;
    private bool canTeleport;

    #endregion

    private void Awake()
    {
        rayVisual = GetComponent<LineRenderer>();
        reticle = Instantiate(teleportMarker);
        reticle.SetActive(false);
        height = headTransform.position.y;

    }

    private void Start()
    {
        if (GameManager.gameManager.virtualReality == true)
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        rayVisual.enabled = false;
        reticle.SetActive(false);
    }

    private void Update()
    {
        ShowLaser();
    }

    public void LaserActivation(bool _State)
    { //Used to turn on and turn off the laser function;
        if (_State == true)
        { //If the laser should be activated;
            rayVisual.enabled = true;
            reticle.SetActive(true);
            canTeleport = true;
            rayVisual.useWorldSpace = true;
            return;
        }
        else

        if (_State == false)
        { //If the laser should be activated;
            rayVisual.enabled = false;
            reticle.SetActive(false);
            canTeleport = false;
            return;
        }
    }

    public void ShowLaser()
    {
        rayVisual.enabled = true;

        if (this.enabled == true)
        {
            RaycastHit hit;

            Debug.DrawRay(transform.position, transform.forward);
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, reticleMask))
            {
                reticle.transform.position = hit.point + new Vector3(0, 0.05f, 0);

                rayVisual.SetPositions(new Vector3[] { transform.position, hit.point });

                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, teleportMask))
                {
                    print(hit.transform.gameObject);
                    if (reticle.activeSelf == false)
                    reticle.SetActive(true);
                    rayVisual.material.color = Color.green;

                    bool input = (GameManager.gameManager.virtualReality == true) ? controller.GetHairTriggerDown() : Input.GetKeyDown("e");

                    if (input)
                        Teleport(hit);
                }
                else
                {
                    if(reticle.activeSelf == true)
                    reticle.SetActive(false);
                    rayVisual.material.color = Color.red;
                }
            }
        }
    }

    void Teleport(RaycastHit hit)
    { 
            Narrative.narrative.teleported = true;
            RegionManager.regionManager.alpha.a = 1; //Fade effect per teleport;
            headTransform.position = hit.point;
            AudioManager.audioManager.PlayAudio(footsteps, transform);
    }
}
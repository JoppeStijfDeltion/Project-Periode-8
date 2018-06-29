using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{

    [Header("Sound:")]
    public AudioClip footsteps;

    [Header("Camera Head Rig Object:")]
    public Transform rig;
    public Transform head;

    [Header("Ray Reference:")]
    public LineRenderer rayVisual;

    [Header("Prefab of the reticle shown when teleport laser is active")]
    public GameObject teleportMarker;
    public Transform worldspace;

    [Header("Allow Teleport Mask:")]
    public LayerMask teleportMask;
    public LayerMask negativeObjects;

    [HideInInspector]
    public float height;
    float heightGet { get { return rig.position.y; } }
    #region Get Controller (Input)

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    #endregion

    #region  Teleport

    private GameObject reticle;
    private bool canTeleport;

    #endregion

    private void Awake()
    {
        reticle = Instantiate(teleportMarker);
        reticle.transform.SetParent(worldspace);
        reticle.SetActive(false);
        height = heightGet;

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
        bool input = (GameManager.gameManager.virtualReality == true) ? controller.GetHairTriggerDown() : Input.GetKeyDown("e");
        rayVisual.enabled = true;

        if (this.enabled == true)
        {
            RaycastHit hit;

            Debug.DrawRay(transform.position, transform.forward);
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, negativeObjects))
            {

                print("Your teleport ray was blocked by: " + hit.transform.gameObject);

                rayVisual.SetPositions(new Vector3[] { transform.position, hit.point });
                reticle.SetActive(false);
                rayVisual.material.color = Color.red;
                return;
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, teleportMask))
            {

                rayVisual.SetPositions(new Vector3[] { transform.position, hit.point });
                reticle.transform.position = hit.point + new Vector3(0, 0.05f, 0);

                if (reticle.activeSelf == false)
                    reticle.SetActive(true);
                rayVisual.material.color = Color.green;

                if (input)
                    Teleport(hit);
            }
        }

        if (reticle.GetComponent<TeleportRoomCheck>().canTeleport == false) rayVisual.material.color = Color.red;
    }

    void Teleport(RaycastHit _hit)
    {
        if (reticle.GetComponent<TeleportRoomCheck>().canTeleport)
        {
            Narrative.narrative.teleported = true;
            RegionManager.regionManager.alpha.a = 1; //Fade effect per teleport;
            Vector3 _Offset = rig.transform.position - head.transform.position;
            Vector3 _Plus = reticle.transform.position + _Offset;
            Vector3 _Final = new Vector3(_Plus.x, height, _Plus.z);
            rig.transform.position = _Final;

            //GameObject _Sphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), rig.transform.position, Quaternion.identity);
            /*PhysicMaterial _Bounce = new PhysicMaterial
            {
            bounciness = 1,
            bounceCombine = PhysicMaterialCombine.Maximum
            };
            _Sphere.AddComponent<Friction>();
            _Sphere.AddComponent<Rigidbody>();
            _Sphere.GetComponent<SphereCollider>().material = _Bounce; */

            //print("Transform Difference: " +(rig.transform.position - _Sphere.transform.position));

            AudioManager.audioManager.PlayAudio(footsteps, transform);
        }
    }
}

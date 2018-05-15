using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent (typeof (CharacterController), typeof (CapsuleCollider))]
public class NonVR : MonoBehaviour {

    // Start settings
    [Header ("Use VR or not")]
    public bool use;
    [Tooltip ("Show a popup everything you start the game asking if you want to use VR or not!")]
    public bool popup = true;

    // Input modification
    [Header ("The speed you move arround")]
    public float movementSpeed = 3f;

    // Ref
    private GameObject cam;
    private CharacterController con;
    private CapsuleCollider col;

    private void Start () {
        con = GetComponent<CharacterController> ();
        col = GetComponent<CapsuleCollider> ();

        // Show popup
        if (popup) {
            if (EditorUtility.DisplayDialog ("Play Mode", "Do you want to use VR mode or Non-VR mode?", "VR Mode", "Non-VR mode"))
                use = true;
            else use = false;
        }

        // Toggle VR
        if (use)
            XRSettings.LoadDeviceByName ("OpenVR");
        // Disable the TrackedPoseDriver so you can move the camera
        else {
            cam = Camera.main.gameObject;
            cam.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver> ().enabled = false;

        }
    }

    private void FixedUpdate () {

        // Update the collider height
        col.height = transform.GetChild (0).transform.position.y;
        col.center = new Vector3 (0f, col.height / 2, 0f);

        // Update the controller height
        con.height = transform.GetChild (0).transform.position.y;
        con.center = new Vector3 (0f, col.height / 2, 0f);

        if (!use) {
            Movement ();
            Rotation ();
        }
    }

    private void Movement () {
        float xInput = Input.GetAxis ("Horizontal");
        float yInput = Input.GetAxis ("Vertical");

        // Gets the input and converts it to world position instead if of local
        Vector3 input = transform.TransformDirection (new Vector3 (xInput, 0, yInput) * movementSpeed);
        // Tell the controller to move
        con.Move (input * Time.deltaTime);
    }

    private void Rotation () {
        float xInput = Input.GetAxis ("Mouse X");
        float yInput = Input.GetAxis ("Mouse Y");

        if (Input.GetButton ("Fire2")) {
            // Rotate the body
            transform.eulerAngles += new Vector3 (0, xInput, 0);
            // Rotate the camera
            cam.transform.eulerAngles += new Vector3 (-yInput, 0, 0);
        }
    }
}
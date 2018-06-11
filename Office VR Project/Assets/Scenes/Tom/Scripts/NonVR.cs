using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent (typeof (CharacterController))]
public class NonVR : MonoBehaviour {

    // Start settings
    [Header ("Use VR or not")]
    public bool use;
    [Tooltip ("Show a popup everytime you start the game asking if you want to use VR or not!")]
    public bool popup = true;

    public GameObject cam;

    [Header ("Hold right click to rotate")]
    public bool rightClick = false;

    // Input modification
    [Header ("The speed you move arround")]
    public float movementSpeed = 3f;

    private float pitch;
    public float rotationSpeed = 2.5f;
    public float clamp = 40f;

    // Ref
    private CharacterController con;

    private void Awake () {
        con = GetComponent<CharacterController> ();
        // Show popup
        if (popup) {
            if (EditorUtility.DisplayDialog ("Play Mode", "Do you want to use VR mode or Non-VR mode?", "VR Mode", "Non-VR mode")) {
                use = true;
                GameManager.gameManager.virtualReality = true;
            }
            else {
                use = false;
                GameManager.gameManager.virtualReality = false;
            }

        }

        // Toggle VR
        if (use) {
            XRSettings.LoadDeviceByName ("OpenVR");
            con.enabled = false;
        }
        else {
            cam.transform.localPosition = new Vector3 (0f, 1.6f, 0f);

            con.enabled = true;
            con.height = 1.6f;
            con.radius = 0.4f;
            con.center = new Vector3 (0f, 0.8f, 0f);
            con.skinWidth = 0.0001f;
        }
    }

    private void Update () {
        if (!use) {
            Rotation ();
        }
    }
    private void FixedUpdate () {
        if (!use) {
            Movement ();
        }
    }

    private void Movement () {
        float xInput = Input.GetAxis ("Horizontal");
        float yInput = Input.GetAxis ("Vertical");

        // Gets the input and converts it to world position instead if of local
        Vector3 input = transform.TransformDirection (new Vector3 (xInput, 0, yInput)* movementSpeed);
        // Tell the controller to move
        con.Move (input * Time.deltaTime);
    }

    private void Rotation () {
        pitch -= rotationSpeed * Input.GetAxis ("Mouse Y");
        pitch = Mathf.Clamp (pitch, -clamp, clamp);

        if (Input.GetButton ("Fire2")|| !rightClick) {
            // Rotate the body
            transform.eulerAngles += new Vector3 (0, Input.GetAxis ("Mouse X")* rotationSpeed, 0);
            // Rotate the camera
            cam.transform.eulerAngles = new Vector3 (pitch, transform.eulerAngles.y, 0);
        }
    }
}
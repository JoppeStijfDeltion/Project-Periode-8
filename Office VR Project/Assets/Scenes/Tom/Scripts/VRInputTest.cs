using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputTest : MonoBehaviour {

    [Header ("X = left, Y = right")]
    public Vector2 trackpadVertical;
    public Vector2 trackpadHorizontal;
    public Vector2 triggerSqueeze;
    public Vector2 gripButton;

    public void Update () {
        #region Buttons
        // Menu
        if (Input.GetButtonDown ("OpenVR_L_Button_Menu")) {
            Debug.Log ("<color=blue>Menu</color>");
        }
        if (Input.GetButtonDown ("OpenVR_R_Button_Menu")) {
            Debug.Log ("<color=red>Menu</color>");
        }

        // Trackpad Press
        if (Input.GetButtonDown ("OpenVR_L_Button_Trackpad_Press")) {
            Debug.Log ("<color=blue>Trackpad_Press</color>");
        }
        if (Input.GetButtonDown ("OpenVR_R_Button_Trackpad_Press")) {
            Debug.Log ("<color=red>Trackpad_Press</color>");
        }

        // Trackpad Touch
        if (Input.GetButtonDown ("OpenVR_L_Button_Trackpad_Touch")) {
            Debug.Log ("<color=blue>Trackpad_Touch</color>");
        }
        if (Input.GetButtonDown ("OpenVR_R_Button_Trackpad_Touch")) {
            Debug.Log ("<color=red>Trackpad_Touch</color>");
        }

        // Trigger Touch
        if (Input.GetButtonDown ("OpenVR_L_Button_Trigger_Touch")) {
            Debug.Log ("<color=blue>Trigger_Touch</color>");
        }
        if (Input.GetButtonDown ("OpenVR_R_Button_Trigger_Touch")) {
            Debug.Log ("<color=red>Trigger_Touch</color>");
        }
        #endregion
        #region Axes
        trackpadVertical = new Vector2 (Input.GetAxis ("OpenVR_L_Axis_Trackpad_Vertical"), Input.GetAxis ("OpenVR_R_Axis_Trackpad_Vertical"));
        trackpadHorizontal = new Vector2 (Input.GetAxis ("OpenVR_L_Axis_Trackpad_Horizontal"), Input.GetAxis ("OpenVR_R_Axis_Trackpad_Horizontal"));
        triggerSqueeze = new Vector2 (Input.GetAxis ("OpenVR_L_Axis_Trigger_Squeeze"), Input.GetAxis ("OpenVR_R_Axis_Trigger_Squeeze"));
        gripButton = new Vector2 (Input.GetAxis ("OpenVR_L_Axis_GripButton"), Input.GetAxis ("OpenVR_R_Axis_GripButton"));
        #endregion
    }
}
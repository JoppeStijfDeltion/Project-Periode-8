using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class RenderLine : MonoBehaviour {

	public bool showLine;
	public Material material;
	public static Transform spawn;
	public static Vector3 target;

	private LineRenderer lineRender;
	private Vector3[] positions = new Vector3[2];

	private void Start () {
		lineRender = GetComponent<LineRenderer> ();
		lineRender.material = material;

		lineRender.enabled = showLine;
	}

	private void Update () {
		// Check if the mouse button is being held
		if (Input.GetButton ("Fire1")) {
			showLine = true;
		}
		else showLine = false;
		// Enable the LineRender component
		if (showLine != lineRender.enabled) {
			lineRender.enabled = showLine;
		}
		// Update the LineRender
		UpdateLineRender ();
	}

	private void UpdateLineRender () {
		positions[0] = spawn.position;
		positions[1] = target;
		lineRender.SetPositions (positions);
	}
}
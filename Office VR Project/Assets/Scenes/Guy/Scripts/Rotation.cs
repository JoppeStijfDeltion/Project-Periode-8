using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    
    [Header("Settings:")]
    public float rotSpeedX, rotSpeedY, rotSpeedZ;
	
	void Update () {
        Rotate();
	}

    void Rotate()
    {
        transform.Rotate(rotSpeedX, rotSpeedY, rotSpeedZ);
    }
}

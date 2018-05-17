using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*For every object that can be picked up*/

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour {

    [Header("Info")]
    public bool beingCarried = false;
	
}

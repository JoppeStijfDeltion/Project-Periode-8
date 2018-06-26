using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour {

    [Header("Settings:")]
    public GameObject waterBottle;
    public GameObject light;

    #region Private Variables
    private Material lightCol;
    private Animator anim;
    #endregion

    void Start () {
        anim = GetComponent<Animator>();
        lightCol = light.GetComponent<MeshRenderer>().material;
	}
	
    public void Open() {
        anim.SetTrigger("Open");
        waterBottle.SetActive(true);
        lightCol.color = Color.green;
        lightCol.SetColor("_EmissionColor", Color.green);
    }
}

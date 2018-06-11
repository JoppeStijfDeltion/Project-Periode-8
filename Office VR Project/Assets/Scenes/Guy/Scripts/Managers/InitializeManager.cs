using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeManager : MonoBehaviour {

    public static InitializeManager initializeManager;

    [Header("Starting Settings:")]
    public float timeTillInteraction;
    public float timeTillFadeIn;

    [Header("Elevator Settings:")]
    public GameObject[] elevatorButtons;
    public Material elevatorButtonActive;
    public Animator elevatorAnimator;
    public AudioClip elevatorOpeningSound;

    public static InitializeManager Initialize {
        get {
            if(initializeManager == null) {
             initializeManager = FindObjectOfType(typeof(InitializeManager)) as InitializeManager;
            }

            if(initializeManager == null) {
                GameObject manager =  Instantiate(new GameObject("InitializeManager"));
                initializeManager = manager.AddComponent(typeof(InitializeManager)) as InitializeManager;
                Debug.Log("No InitializeManager could be found, instancing new InitializeManager...");
                }

            return initializeManager;
        }
    }

    private void OnApplicationQuit() {
        initializeManager = null;
    }

    public void Awake() {
         initializeManager = Initialize;
    }

    public void Start() {
        foreach(PickupSystem _Hand in GameManager.gameManager.hands) {
            _Hand.enabled = false;
        }

        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame() {
        yield return new WaitForSeconds(timeTillFadeIn); //When the game fades in;
        RegionManager.regionManager.fade = false;
        StartCoroutine(ElevatorSoundQueue());

        yield return new WaitForSeconds(timeTillInteraction);

        foreach(PickupSystem _Hand in GameManager.gameManager.hands) {
            _Hand.enabled = true;
        }

        elevatorAnimator.SetTrigger("Open");
        GameManager.gameManager.startedGame = true;
        foreach(GameObject _Button in elevatorButtons) {
        _Button.GetComponent<MeshRenderer>().material = elevatorButtonActive;
        }
    }

    private IEnumerator ElevatorSoundQueue() {
        yield return new WaitForSeconds(2.5f);
        AudioManager.audioManager.PlayAudio(elevatorOpeningSound, transform);
    }
}

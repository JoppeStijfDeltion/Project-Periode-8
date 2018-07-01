using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrative : MonoBehaviour {

	static public Narrative narrative;

	IEnumerator StartNarrative { get { yield return new WaitForSeconds(timeTillStart); StartCoroutine(Begin()); }}

	public enum Acts	{ActOne, ActTwo, ActThree, finalAct};
	public Acts         narrativeState;

	enum Objective {Default, Ray, Teleport, Throw};
	Objective objectiveState;

	[Header("References:")]
	public Text subtitles;
	public GameObject testItem;

	[Header("Initialize Settings:")]
	public float 		timeTillStart = 4;
	public AudioClip    phoneringing;

	[Header("Timers:")]
	public float[]		firstArcTimers;
	public float[]		secondArcTimers;
	public float[] 		thirdArcTimers;
	public float[]		finalArcTimers;

	[Header("Text Containers:")]
	public string[]     firstAct;
	public string[]     secondAct;
	public string[]     thirdAct;
	public string[]		finalAct; //No extras;

	[Header("Extra Containers:")]
	public string[]     firstActExtra;
	public string[]     secondActExtra;
	public string[]     finalActExtra;

	[Header("Audio Container:")]
	public AudioClip[]  voiceContainer;
	public AudioClip[]  voiceExtraOne;
	public AudioClip[] 	voiceExtraTwo;
	public AudioClip[]	voiceExtraThree;

	[Header("Random Audio Queue Settings:")]
	public float timeTill;

	[HideInInspector]
	public bool 		teleported;

	#region Private Variables
	AudioSource 		aSource;
	int					audioIndex;
	int					dialogueIndex;
	float				timer;
	bool				started = false;

	#endregion

	void Awake() {
		if(narrative == null)
		narrative = this;
		else
		Destroy(this);

		aSource = GetComponent<AudioSource>();
		IEnumerator _Start = StartNarrative;
		StartCoroutine(_Start);
		timer = timeTill;
	}

    void Start()
    {
        InitializeManager.initializeManager.radio.SetActive(false);    
    }

    public void Update() {
		NarrativeProgression();
		Check();
		RandomSound();
	}

	void RandomSound() {
	timer -= Time.deltaTime;
	int _Index;

	if(timer <= 0) { 
		switch(objectiveState) {
			case Objective.Ray:
			_Index = Random.Range(0, voiceExtraOne.Length);
			SetAudio(null, voiceExtraOne , _Index);
			StartCoroutine(SetDialogue(firstActExtra[_Index], null, null));
			break;

			case Objective.Teleport:
			_Index = Random.Range(0, voiceExtraTwo.Length);
			SetAudio(null, voiceExtraTwo , _Index);
			StartCoroutine(SetDialogue(secondActExtra[_Index], null, null));
			break;

			case Objective.Throw:
			_Index = Random.Range(0, voiceExtraThree.Length);
			SetAudio(null, voiceExtraThree , _Index);
			StartCoroutine(SetDialogue(finalActExtra[_Index], null, null));

			break;
			}
			timer = timeTill;
		}
	}

	IEnumerator SetDialogue(string _I, string[] _DiaList, float[] _Timers) {
		if(_I != null) {
		subtitles.text = _I;
		yield return new WaitForSeconds(aSource.clip.length);
		}


		if(_DiaList != null) {
			for(int i = 0; i < _DiaList.Length; i++) {
				subtitles.text = _DiaList[i];
				yield return new WaitForSeconds(_Timers[i]);
			}
		}

		subtitles.text = "";
	}

	void Check() {
		if(objectiveState != Objective.Teleport)
		teleported = false;

		switch(objectiveState) {
			case Objective.Ray:
			foreach(PickupSystem _Hand in GameManager.gameManager.hands) {
				if(_Hand.enabled == true) {
				if(_Hand.interactionState == PickupSystem.Interaction.RayInteraction) {
					objectiveState = Objective.Default;
					narrativeState = Acts.ActTwo;
					dialogueIndex = 0;
                            StopCoroutine(NarrativeProgression());
                            StartCoroutine(NarrativeProgression());
					}
				}
			}
			break;

			case Objective.Teleport:
			if(teleported == true) {
				objectiveState = Objective.Default;
				narrativeState = Acts.ActThree;
				dialogueIndex = 0;
                    StopCoroutine(NarrativeProgression());
                    StartCoroutine(NarrativeProgression());
			}
			break;

			case Objective.Throw:
			foreach(PickupSystem _Hand in GameManager.gameManager.hands) {
				if(_Hand.enabled == true) { 
				if(_Hand.objectBeingCarried != null) {
				if((GameManager.gameManager.virtualReality == true)?  _Hand.Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger): Input.GetKeyUp(KeyCode.E)) { //Checks if you are holding down the button;
				objectiveState = Objective.Default;
				narrativeState = Acts.finalAct;
				dialogueIndex = 0;
                            StopCoroutine(NarrativeProgression());
                            StartCoroutine(NarrativeProgression());
					}
				}
			}
		}
			break;
		}
	}

	IEnumerator Begin() {
		if (GameManager.gameManager.playCutscene == true)
        {
		SetAudio(phoneringing, null, 0);
		yield return new WaitForSeconds(aSource.clip.length);
		StartCoroutine(NarrativeProgression());
		}
		else {
			if(started == false) {
				started = true;
			StartCoroutine(InitializeManager.initializeManager.StartGame());
            RegionManager.regionManager.fade = true;
			}
		}
	}

	IEnumerator NarrativeProgression() {
            switch (narrativeState)
            {
                case Acts.ActOne:
                    audioIndex = 0;
                    SetAudio(voiceContainer[audioIndex], null, 0);
                    StopCoroutine(SetDialogue(null, null, null));
                    StartCoroutine(SetDialogue(null, firstAct, firstArcTimers));
                    yield return new WaitForSeconds(aSource.clip.length);
                    objectiveState = Objective.Ray;
                    break;

                case Acts.ActTwo:
                    audioIndex = 1;
                    SetAudio(voiceContainer[audioIndex], null, 0);
                    StopCoroutine(SetDialogue(null, null, null));
                    StartCoroutine(SetDialogue(null, secondAct, secondArcTimers));
                    yield return new WaitForSeconds(aSource.clip.length);
                    objectiveState = Objective.Teleport;
                    break;

                case Acts.ActThree:
                    audioIndex = 2;
                    SetAudio(voiceContainer[audioIndex], null, 0);
                    StopCoroutine(SetDialogue(null, null, null));
                    StartCoroutine(SetDialogue(null, thirdAct, thirdArcTimers));
                    yield return new WaitForSeconds(aSource.clip.length);
                    testItem.GetComponent<MeshRenderer>().material.SetFloat(("_interact"), 1);
                    objectiveState = Objective.Throw;
                    break;

                case Acts.finalAct:
                    audioIndex = 3;
                    SetAudio(voiceContainer[audioIndex], null, 0);
                    StartCoroutine(SetDialogue(null, finalAct, finalArcTimers));
                    yield return new WaitForSeconds(aSource.clip.length);
                    audioIndex = 4;
                    SetAudio(voiceContainer[audioIndex], null, 0);
                    RegionManager.regionManager.fade = true;
                    foreach (PickupSystem _Hand in GameManager.gameManager.hands)
                    {
                        if (_Hand.objectBeingCarried != null)
                        {
                            if (_Hand.objectBeingCarried.GetComponent<Friction>())
                                _Hand.Throwing();
                        }
                    }

                    if (started == false)
                    {
                        started = true;
                        StartCoroutine(InitializeManager.initializeManager.StartGame());
                    }
                    break;
            }
        } 

	#region Audio
	void SetAudio(AudioClip _Clip, AudioClip[] _Acts, int _Index) {
		if(_Acts == null) {
		aSource.clip = _Clip;
		}
		else {
			aSource.clip = _Acts[_Index];
		}

		aSource.Play();
	}
	#endregion
}

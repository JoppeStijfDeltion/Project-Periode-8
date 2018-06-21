using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

		public static EffectManager effectManager;
	    public static EffectManager Initialize {
        get {
            if(effectManager == null) {
             effectManager = FindObjectOfType(typeof(EffectManager)) as EffectManager;
            }

            if(effectManager == null) {
                GameObject manager =  Instantiate(new GameObject("EffectManager"));
                effectManager = manager.AddComponent(typeof(EffectManager)) as EffectManager;
                Debug.Log("No InitializeManager could be found, instancing new EffectManager...");
                }

            return effectManager;
        }
    }

	[System.Serializable]
	public struct Effects{
		[Header("Settings:")]
		public string _Name;
		public GameObject _Particles;
		public AudioClip _Sound;
	}

	public List<Effects> visualEffects;

    private void OnApplicationQuit() {
        effectManager = null;
    }

    public void Awake() {
         effectManager = Initialize;
    }

	public void InstantiateEffect(string _Indentifier, Transform _Pos) {
		foreach(Effects _Effect in visualEffects) { //Goes through every possible effect;
			if(_Effect._Name == _Indentifier) { //If the CGI effect has been found;
				Vector3 _NewPos = new Vector3(_Pos.position.x, _Pos.position.y, _Pos.position.z - 0.2f);
				GameObject _Instance = Instantiate(_Effect._Particles, _NewPos, Quaternion.identity); //Instantiates the particles;
				Destroy _DestroyTime = _Instance.AddComponent<Destroy>();
				_DestroyTime.StartCoroutine(_DestroyTime.DestroySelf(_Effect._Particles.GetComponent<ParticleSystem>().main.duration * 2));
				if(_Effect._Sound != null) //If there is audio to be played;
				AudioManager.audioManager.PlayAudio(_Effect._Sound, _Pos); //Plays the sound;
				return;
			}
		}
	}
}

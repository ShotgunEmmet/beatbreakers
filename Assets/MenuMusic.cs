using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {
	
	private static MenuMusic instance = null;
	private AudioSource audioSource;
	private bool playMusic = true;
	
	public static MenuMusic Instance{
		get { return instance; }
	}
	
	void Start(){
		audioSource = GetComponent(typeof(AudioSource)) as AudioSource;
		if(!GameGlobals.sound)
			audioSource.enabled = false;
	}
	
	
	void Awake(){
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		}
		else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Update(){
		if(playMusic)
			if(GameGlobals.sound != audioSource.enabled)
				audioSource.enabled = GameGlobals.sound;
	}
	
	public void SetEnabled(bool state){
		playMusic = state;
		if(!playMusic)
			audioSource.enabled = playMusic;
	}
	
}

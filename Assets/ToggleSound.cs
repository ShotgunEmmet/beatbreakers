using UnityEngine;
using System.Collections;

public class ToggleSound : MonoBehaviour {
	
	public GameObject on, off;
	public UILabel label;
	
	// Use this for initialization
	void Start () {
		Toggle();
	}
	
	void OnClick(){
		GameGlobals.sound = !GameGlobals.sound;
		Toggle();
	}
	
	void Toggle(){
		if(GameGlobals.sound){
			on.SetActive(true);
			off.SetActive(false);
			label.text = "Sound\nOn";
			
			foreach(UIButtonSound soundMaker in GameObject.FindObjectsOfType(typeof(UIButtonSound))){
				((UIButtonSound)soundMaker).volume = 1f;
			}
		}
		else{
			on.SetActive(false);
			off.SetActive(true);
			label.text = "Sound\nOff";
			
			foreach(UIButtonSound soundMaker in GameObject.FindObjectsOfType(typeof(UIButtonSound))){
				((UIButtonSound)soundMaker).volume = 0f;
			}
		}
		//(GetComponent(typeof(AudioListener)) as AudioListener).enabled = GameGlobals.sound;
		PlayerPrefs.SetInt("Sound", GameGlobals.sound ? 0 : 1);
	}
}

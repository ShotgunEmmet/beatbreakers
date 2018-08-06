using UnityEngine;
using System.Collections;

public class MuteSound : MonoBehaviour {
	
	void Start () {
		if(!GameGlobals.sound){
			StartCoroutine(SetAudio());
			
			foreach(UIButtonSound soundMaker in GameObject.FindObjectsOfType(typeof(UIButtonSound))){
				((UIButtonSound)soundMaker).volume = 0f;
			}
		}
	}
	
	IEnumerator SetAudio(){
		while(GetComponent(typeof(AudioListener)) == null)
			yield return new WaitForSeconds(0f);
		(GetComponent(typeof(AudioListener)) as AudioListener).enabled = GameGlobals.sound;
		yield return null;
	}
}

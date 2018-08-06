using UnityEngine;
using System.Collections;

public class ToggleMessages : MonoBehaviour {

	public GameObject on, off;
	public UILabel label;
	
	// Use this for initialization
	void Start () {
		Toggle();
	}
	
	void OnClick(){
		GameGlobals.messages = !GameGlobals.messages;
		Toggle();
	}
	
	void Toggle(){
		if(GameGlobals.messages){
			on.SetActive(true);
			off.SetActive(false);
			label.text = "Show\nMessages";
		}
		else{
			on.SetActive(false);
			off.SetActive(true);
			label.text = "Hide\nMessages";
		}
		PlayerPrefs.SetInt("Messages", GameGlobals.messages ? 0 : 1);
	}
	
}

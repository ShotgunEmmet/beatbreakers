using UnityEngine;
using System.Collections;

public class MultiplierDisplay : MonoBehaviour {
	
	UILabel label;
	private bool visible = false;
	// Use this for initialization
	void Start () {
		label = (UILabel)GetComponent(typeof(UILabel));
		label.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(!visible){
			if(GameGlobals.MultiplierDuration > 0f){
				visible = true;
				label.text = "2X";
			}
		}
		else{
			if(GameGlobals.MultiplierDuration <= 0f){
				visible = false;
				label.text = "";
			}
		}
			
	}
}

using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {
	
	UILabel time;
	
	// Use this for initialization
	void Start () {
		time = ( (UILabel) GetComponent(typeof(UILabel)) );
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameGlobals.timeRemaining > 0f)
			time.text = Mathf.FloorToInt(GameGlobals.timeRemaining/60f).ToString("00") + ":" + Mathf.FloorToInt(GameGlobals.timeRemaining%60f).ToString("00");
		else
			time.text = "00:00";
	}
}

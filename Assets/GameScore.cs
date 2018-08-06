using UnityEngine;
using System.Collections;

public class GameScore : MonoBehaviour {
	
	private UILabel score;
	
	// Use this for initialization
	void Start () {
		score = ( (UILabel) GetComponent(typeof(UILabel)) );	
	}
	
	// Update is called once per frame
	void Update () {
		score.text = "Score "+GameGlobals.currentScore;
	}
}

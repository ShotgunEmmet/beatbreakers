using UnityEngine;
using System.Collections;

public class OpponentsScore : MonoBehaviour {

	private UILabel opponentsScore;
	
	// Use this for initialization
	void Start () {
		opponentsScore = ( (UILabel) GetComponent(typeof(UILabel)) );
		if(GameGlobals.gameState == GameGlobals.GameState.Challenge){
			if(GameGlobals.opponentsScore >= 0)
				opponentsScore.text = "OPPONENTS SCORE\n"+GameGlobals.opponentsScore.ToString("00000000");
			else
				opponentsScore.text = "OPPONENTS SCORE\n--------";
		}
		else{
			gameObject.SetActive(false);
		}
	}
	
}

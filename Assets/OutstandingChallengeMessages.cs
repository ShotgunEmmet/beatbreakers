using UnityEngine;
using System.Collections;

public class OutstandingChallengeMessages : MonoBehaviour {
	public UILabel label;
	// Use this for initialization
	void Start () {
		//Pierce/ Shouldn't this be a gamespark call & not a playerpref (assuming the number is unaccepted challenges)
		label.text = PlayerPrefs.GetInt("challengeMessages",0).ToString(); //GameGlobals.challengeMessages.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

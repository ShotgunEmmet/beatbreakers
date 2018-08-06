using UnityEngine;
using System.Collections;

public class PauseReplay : MonoBehaviour {
 	GameSparks GSApi;
	// Use this for initialization
	void Start () {
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
	}
	void OnClick(){
		 Debug.LogWarning("posting highscore");
		GSApi.logChallengeScore( GameGlobals.relevantID,GameGlobals.currentScore,GameGlobals.selectedTrack.challengeShortCode);
	}
	// Update is called once per frame
	void Update () {
	
	}
}

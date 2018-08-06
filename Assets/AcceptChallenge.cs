using UnityEngine;
using System.Collections;

public class AcceptChallenge : MonoBehaviour {
	GameSparks GSApi; 
	void OnClick(){
		//pierce/ change the status of this challenge to accepted (GameGlobals.relivantID should still be set)
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		GSApi.acceptChallenge(GameGlobals.relevantID,"");
		GameGlobals.loading = true;
		Application.LoadLevelAsync("ChallengesList");
	}
}

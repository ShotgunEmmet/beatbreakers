using UnityEngine;
using System.Collections;

public class RejectChallenge : MonoBehaviour {
	public GameSparks GSApi;
	void OnClick(){
		//pierce/ change the status of this challenge to rejected (GameGlobals.relivantID should still be set)
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		GSApi.declineChallenge(GameGlobals.relevantID, null);
		GameGlobals.loading = true;
		Application.LoadLevelAsync("ChallengesList");
	}
}

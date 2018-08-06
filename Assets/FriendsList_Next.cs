using UnityEngine;
using System.Collections;

public class FriendsList_Next : MonoBehaviour {
	
	void OnClick()
	{
		//load ChallengesList
		if(GameGlobals.otherIDs.Count > 0){
			GameGlobals.loading = true;
			Application.LoadLevelAsync("DesignChallenge");
		}
	}
}

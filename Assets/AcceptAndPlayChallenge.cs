using UnityEngine;
using System.Collections;

public class AcceptAndPlayChallenge : MonoBehaviour {
	public GameSparks GSApi;
	
	void Start()
	{
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		

		
	}
	void OnClick(){
		//pierce/ change the status of this challenge to accepted (GameGlobals.relivantID should still be set)
		 Debug.LogError("not correct use");
		GSApi.acceptChallenge(GameGlobals.relevantID, null);
		
		//set global variables to the challenges id & load challenge game
		//GameGlobals.opponentsScore = <challengersScore>
		//GameGlobals.relevantID = <challengeID>
		
		GameGlobals.gameState = GameGlobals.GameState.Challenge;
		GameGlobals.loading = true;
		MenuMusic.Instance.SetEnabled(false);
		Application.LoadLevelAsync("Game");
	}
}

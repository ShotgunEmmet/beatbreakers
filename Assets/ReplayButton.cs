using UnityEngine;
using System.Collections;

public class ReplayButton : MonoBehaviour {
	
	public UILabel areYouSure;
	public GameObject yesButton, noButton;
	public bool clicked = false;
	
	void OnClick(){
		if(areYouSure == null){
			Load();
		}
		else{
			clicked = true;
			
			areYouSure.gameObject.SetActive(true);
			yesButton.SetActive(true);
			noButton.SetActive(true);
			
			if(GameGlobals.gameState == GameGlobals.GameState.Journey){
				areYouSure.text = "Are you sure?";
			}
			else if(GameGlobals.gameState == GameGlobals.GameState.AgainstTheClock){
				areYouSure.text = "Are you sure?";
			}
			else if(GameGlobals.gameState == GameGlobals.GameState.Challenge){
				areYouSure.text = "Submit & Retry?";
			}
		}
		
	}
	
	public void Load(){
		if(areYouSure != null){
			GameSparks GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
				
			 Debug.LogWarning("posting score to challenge");
			GSApi.logChallengeScore( GameGlobals.relevantID,GameGlobals.currentScore,GameGlobals.selectedTrack.challengeShortCode);
			GameGlobals.challengeAttemptsRemaining--;
			
			if(GameGlobals.gameState == GameGlobals.GameState.AgainstTheClock){
				//Piere/ send score to highscore table
				if(!GSApi.OfflineMode())
					GSApi.logHighScore(GameGlobals.currentScore, GameGlobals.selectedTrack.challengeShortCode);
			}
		}
		
		if(GameGlobals.currentScore > GameGlobals.opponentsScore)
			GameGlobals.opponentsScore = GameGlobals.currentScore;
				
		GameGlobals.loading = true;
		
		Application.LoadLevel(Application.loadedLevelName);
	}
	
	public void Hide(){
		clicked = false;
	
		areYouSure.gameObject.SetActive(false);
		yesButton.SetActive(false);
		noButton.SetActive(false);
	}
}

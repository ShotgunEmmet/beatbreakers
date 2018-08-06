using UnityEngine;
using System.Collections;

public class HomeButton : MonoBehaviour {
	
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
				areYouSure.text = "Submit & Exit?";
			}
		}
		
	}
	
	public void Load(){
		GameSparks GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		
		 Debug.LogWarning("posting score to challenge");
		GSApi.logChallengeScore( GameGlobals.relevantID,GameGlobals.currentScore,GameGlobals.selectedTrack.challengeShortCode);
		GameGlobals.loading = true;
		MenuMusic.Instance.SetEnabled(true);
		if(GameGlobals.gameState == GameGlobals.GameState.Journey)
			Application.LoadLevelAsync("Journey");
		else if(GameGlobals.gameState == GameGlobals.GameState.AgainstTheClock){
		if(!GSApi.OfflineMode())
			GSApi.logHighScore(GameGlobals.currentScore, GameGlobals.selectedTrack.challengeShortCode);
			Application.LoadLevelAsync("BuyTracks");
		}
		else if(GameGlobals.gameState == GameGlobals.GameState.Challenge)
			Application.LoadLevelAsync("ChallengeDetails");
	}
	
	public void Hide(){
		clicked = false;
	
		areYouSure.gameObject.SetActive(false);
		yesButton.SetActive(false);
		noButton.SetActive(false);
	}
}

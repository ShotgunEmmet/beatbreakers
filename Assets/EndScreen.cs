using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {
	
	
	public GameObject[] endScreenElements;
	public GameObject[] endScreenSinglePlayer;
	public GameObject[] endScreenSinglePlayer2;
	public AwardBox awardBox;
	public GameObject[] endScreenAgainstThecClock1;
	public GameObject[] endScreenAgainstThecClock2;
	public GameObject[] endScreenChallenge;
	public GameObject[] journeyTicks;
	public GameObject[] ticks;
	public GameObject[] notEndScreen;
	public UILabel score;
	public UILabel total;
	public UILabel earnedPurse;
	public UILabel oldPurse;
	public UILabel newPurse;
	
	public UILabel target1;
	public UILabel target2;
	public UILabel target3;
	
	public ReplayButton replayButton;
	public NextButton nextButton;
	
	public UILabel c_highscore;
	public UILabel c_score;
	public UILabel c_attempts;
	
	private bool gameOver = false;
	GameSparks GSApi; 
	public int screen = 0;
	
	// Use this for initialization
	void Start () {
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		
		foreach(GameObject obj in endScreenElements)
			obj.SetActive(false);
		
		foreach(GameObject obj in endScreenSinglePlayer)
			obj.SetActive(false);
		
		foreach(GameObject obj in journeyTicks)
			obj.SetActive(false);
		
		foreach(GameObject obj in ticks)
			obj.SetActive(false);
		
		foreach(GameObject obj in endScreenSinglePlayer2)
			obj.SetActive(false);
		
		foreach(GameObject obj in endScreenAgainstThecClock1)
			obj.SetActive(false);
		
		foreach(GameObject obj in endScreenAgainstThecClock2)
			obj.SetActive(false);
		
		foreach(GameObject obj in endScreenChallenge)
			obj.SetActive(false);
		
		target1.text = GameGlobals.challengeScore1.ToString();
		target2.text = GameGlobals.challengeScore2.ToString();
		target3.text = GameGlobals.challengeScore3.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameGlobals.timeRemaining <=0){
			if(!gameOver){
				gameOver = true;
				
				foreach(GameObject obj in notEndScreen)
					obj.SetActive(false);
				
				foreach(GameObject obj in endScreenElements)
					obj.SetActive(true);
				
				if(GameGlobals.gameState == GameGlobals.GameState.Journey)
				{
					foreach(GameObject obj in endScreenSinglePlayer)
						obj.SetActive(true);
					int currencyEarned = 0;
					total.text = "NOTHING   ";
					if(GameGlobals.currentScore >= GameGlobals.challengeScore1){
						journeyTicks[0].SetActive(true);
						currencyEarned+=100;
						awardBox.renderer.material.mainTexture = awardBox.bronze;
						total.text = "BRONZE   ";
					}
					if(GameGlobals.currentScore >= GameGlobals.challengeScore2){
						journeyTicks[1].SetActive(true);
						currencyEarned+=100;
						awardBox.renderer.material.mainTexture = awardBox.silver;
						total.text = "SILVER   ";
					}
					if(GameGlobals.currentScore >= GameGlobals.challengeScore3){
						journeyTicks[2].SetActive(true);
						currencyEarned+=100;
						awardBox.renderer.material.mainTexture = awardBox.gold;
						total.text = "GOLD   ";
					}
					score.text = "SCORE  "+GameGlobals.currentScore.ToString("00000000");
					
					string[] songString = GameGlobals.selectedTrack.song.Split('/');
					if(PlayerPrefs.GetInt(songString[songString.Length-1]+"unlocked")  < (int)(currencyEarned/100)){
						PlayerPrefs.SetInt(songString[songString.Length-1]+"unlocked", (int)(currencyEarned/100));
					}
					
					if(currencyEarned == 0){
						nextButton.nextBackground.SetActive(false);
						nextButton.returnBackground.SetActive(true);
						screen++;
					}
				}
				else if (GameGlobals.gameState == GameGlobals.GameState.AgainstTheClock){
					foreach(GameObject obj in endScreenAgainstThecClock1){
						obj.SetActive(true);
						if(!GameGlobals.online)
							if(obj.name == "LeaderboardButton")
								obj.SetActive(false);
					}
					int currencyEarned = 0;
					if(GameGlobals.currentScore >= GameGlobals.challengeScore1){
						ticks[0].SetActive(true);
						currencyEarned+=100;
					}
					if(GameGlobals.currentScore >= GameGlobals.challengeScore2){
						ticks[1].SetActive(true);
						currencyEarned+=100;
					}
					if(GameGlobals.currentScore >= GameGlobals.challengeScore3){
						ticks[2].SetActive(true);
						currencyEarned+=100;
					}
					total.text = currencyEarned.ToString();
					score.text = "SCORE  "+GameGlobals.currentScore.ToString("00000000");
					earnedPurse.text = currencyEarned.ToString("0");
					oldPurse.text = PlayerPrefs.GetInt("InGameCurrency").ToString();//display currency purse value
					//pierce/ add earned to current currency purse
					PlayerPrefs.SetInt("InGameCurrency", PlayerPrefs.GetInt("InGameCurrency")+currencyEarned);
					newPurse.text = PlayerPrefs.GetInt("InGameCurrency").ToString();//display currency purse value
					
					//Piere/ send score to highscore table
					if(!GSApi.OfflineMode())
						GSApi.logHighScore(GameGlobals.currentScore, GameGlobals.selectedTrack.challengeShortCode);
					
		
					
				}
				else if (GameGlobals.gameState == GameGlobals.GameState.Challenge){
					foreach(GameObject obj in endScreenChallenge)
						obj.SetActive(true);
					if(!GSApi.OfflineMode())
						GSApi.logChallengeScore(GameGlobals.relevantID,GameGlobals.currentScore, GameGlobals.selectedTrack.challengeShortCode);
					
					GameGlobals.challengeAttemptsRemaining--;
					
					if(GameGlobals.challengeAttemptsRemaining <= 0){
						replayButton.gameObject.SetActive(false);
						nextButton.gameObject.transform.localPosition = new Vector3(-135f ,nextButton.gameObject.transform.localPosition.y, nextButton.gameObject.transform.localPosition.z);
					}
					
					nextButton.nextBackground.SetActive(false);
					nextButton.returnBackground.SetActive(true);
					
					if(GameGlobals.opponentsScore >= 0)
						c_highscore.text = GameGlobals.opponentsScore.ToString();
					else
						c_highscore.text = "------";
					
					c_score.text = GameGlobals.currentScore.ToString();
					c_attempts.text = GameGlobals.challengeAttemptsRemaining.ToString();//the number of attempts left;
				}
			}
		}
	}
	
	public void Next(){
		if(GameGlobals.gameState == GameGlobals.GameState.Journey)
		{
			if(screen == 0){
				foreach(GameObject obj in endScreenSinglePlayer)
					obj.SetActive(false);
				foreach(GameObject obj in journeyTicks)
					obj.SetActive(false);
				
				foreach(GameObject obj in endScreenSinglePlayer2)
					obj.SetActive(true);
				
				screen++;
			}
			else{
				GameGlobals.loading = true;
				MenuMusic.Instance.SetEnabled(true);
				Application.LoadLevelAsync("Journey");
			}
		}
		else if (GameGlobals.gameState == GameGlobals.GameState.AgainstTheClock){
			if(screen == 0){
				foreach(GameObject obj in endScreenAgainstThecClock1)
					obj.SetActive(false);
				foreach(GameObject obj in ticks)
					obj.SetActive(false);
				
				foreach(GameObject obj in endScreenAgainstThecClock2)
					obj.SetActive(true);
				
				screen++;
			}
			else{
				GameGlobals.loading = true;
				
				//Pierce/ send the score for this song to the leaderboards
				if(!GSApi.OfflineMode())
					GSApi.logHighScore(GameGlobals.currentScore, GameGlobals.selectedTrack.challengeShortCode);
				//Application.LoadLevelAsync("AgainstTheClock");
				MenuMusic.Instance.SetEnabled(true);
				Application.LoadLevelAsync("BuyTracks");
				
			}
		}
		else if (GameGlobals.gameState == GameGlobals.GameState.Challenge){
			//pierce/ send score to the server (not sure if you only send the highest or each attempts score)
			GameGlobals.loading = true;
			MenuMusic.Instance.SetEnabled(true);
			Application.LoadLevelAsync("ChallengeDetails");
		}
	}
}

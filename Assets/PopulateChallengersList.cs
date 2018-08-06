using UnityEngine;
using System.Collections;

public class PopulateChallengersList : MonoBehaviour {

	public GameObject challengerPrefab;
	public GameSparks GSApi;
	ArrayList challengers;
	
	public GameObject playButton;
	
	// Use this for initialization
	void Start () {
		

		
		challengers = new ArrayList();
		
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		
		Hashtable challengesHash = GSApi.listChallengeDetails(GameGlobals.relevantID);
		
		string shortCode = (string)(challengesHash["challenge"] as Hashtable)["shortCode"];
		 Debug.LogWarning(shortCode);
		
		ArrayList ChallengeResponse = ((challengesHash["challenge"] as Hashtable)["challenged"]) as ArrayList;
		
		Hashtable attemptList = (Hashtable)(((Hashtable)challengesHash["challenge"])["turnCount"]);
		
		
		
		
		foreach(DictionaryEntry currentTrack in GameGlobals.Songs){
			if(((AudioTrack)currentTrack.Value).challengeShortCode == shortCode){
				GameGlobals.selectedTrack = ((AudioTrack)currentTrack.Value);//set track for challenge
				break;
			}
		}
		
		
		
		// Debug.LogWarning(GameGlobals.shortCode );
		GameGlobals.opponentsScore = 0;
		
		ArrayList scores = (ArrayList)(((Hashtable)GSApi.ListChallengeScores(shortCode, GameGlobals.relevantID ,10))["data"]);
		
		Hashtable pScore = new Hashtable();
		
		if(scores != null){
			foreach(Hashtable scoreEnt in scores){
				
				if(GameGlobals.opponentsScore < (double)scoreEnt["HS"])
					GameGlobals.opponentsScore = (int)(double)scoreEnt["HS"];//set high score for challenge
				pScore.Add( (string)((Hashtable)scoreEnt)["userId"], (double)scoreEnt["HS"] );
			}
		}
		int score = 0;
		int attemptCount = 0; 
		
		string id= (string)(( (Hashtable) ( (Hashtable)challengesHash["challenge"] )["challenger"]  )["id"]);
			
		if(attemptList[id] != null){
			attemptCount = (int)(double)attemptList[id];
			if(pScore[id] != null){
				score = (int)(double)pScore[id];
			}
		}
	
		challengers.Add(new Challenger{id =(string)(( (Hashtable) ( (Hashtable)challengesHash["challenge"] )["challenger"]  )["id"]), displayName = (string)(( (Hashtable) ( (Hashtable)challengesHash["challenge"] )["challenger"]  )["name"]), topScore = score,attempts= attemptCount});
			
		
		foreach(Hashtable challenger in ChallengeResponse)
		{
			score = 0;
			attemptCount = 0; 
			
			id= (string)challenger["id"];
			
			if(attemptList[(string)challenger["id"]] != null){
				attemptCount = (int)(double)attemptList[id];
				if(pScore[id] != null){
					score = (int)(double)pScore[id];
				}
			}
			
			
			
			challengers.Add(new Challenger{id = (string)challenger["id"], displayName = (string)challenger["name"], topScore = score,attempts= attemptCount});
			
		}
		
		

		
		//populate the chat windows with the correct chat history
		
		Scroll_Item_ChallengesList.itemCount=1;
		for(int counter = 0; counter < challengers.Count; counter++){
			GameObject item = Instantiate(challengerPrefab,challengerPrefab.transform.position,Quaternion.identity) as GameObject;
			item.transform.parent = this.transform;
			item.transform.localScale = new Vector3(1,1,1);
			item.transform.localPosition = item.transform.position + new Vector3(0,-46*counter,0);
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers)) as Scroll_Item_Challengers).id = ((Challenger)challengers[counter]).id;
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers)) as Scroll_Item_Challengers).name.text = ((Challenger)challengers[counter]).displayName;
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers)) as Scroll_Item_Challengers).topScore.text = ((Challenger)challengers[counter]).topScore.ToString();
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers)) as Scroll_Item_Challengers).attempts.text = ((Challenger)challengers[counter]).attempts.ToString()+ "/" + ((double)(((Hashtable)challengesHash["challenge"])["maxTurns"])).ToString();
			
			if(((Challenger)challengers[counter]).id == GameGlobals.userID){//attempts remaining
				GameGlobals.challengeAttemptsRemaining =  ((int)((double)(((Hashtable)challengesHash["challenge"])["maxTurns"]))) - ((int)((double)((Challenger)challengers[counter]).attempts));
				
				if(GameGlobals.challengeAttemptsRemaining <= 0)
					playButton.SetActive(false);
			}
		}
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}

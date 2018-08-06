using UnityEngine;
using System.Collections;

public class PopulateChallengersFinishedList : MonoBehaviour {

	public GameObject challengerPrefab;
	public GameSparks GSApi;
	ArrayList challengers;
	
	// Use this for initialization
	void Start () {
		challengers = new ArrayList();
		
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		
		
		
		Hashtable challengesHash = GSApi.listChallengeDetails(GameGlobals.relevantID);
		
		
		
		ArrayList ChallengeResponse = ((challengesHash["challenge"] as Hashtable)["challenged"]) as ArrayList;
		
		
		
		Hashtable attemptList = (Hashtable)(((Hashtable)challengesHash["challenge"])["turnCount"]);
		
		ArrayList scores = (ArrayList)(((Hashtable)GSApi.ListChallengeScores(GameGlobals.selectedTrack.challengeShortCode, GameGlobals.relevantID ,10))["data"]);
		
		//GameGlobals.shortCode = (string)(((Hashtable)challengesHash["challenge"])["shortCode"]);
		// Debug.LogWarning(GameGlobals.shortCode );
		
		Hashtable pScore = new Hashtable();
		
		 Debug.LogWarning(((Hashtable)GSApi.ListChallengeScores(GameGlobals.selectedTrack.challengeShortCode, GameGlobals.relevantID ,10))["data"]);
		 Debug.LogWarning(scores.Count);
		
		if(scores != null){
			foreach(Hashtable scoreEnt in scores){
				
				 Debug.LogWarning(
					(scoreEnt) ["HS"]
					);
				 Debug.LogWarning(scoreEnt["userId"]);
				
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
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).id = ((Challenger)challengers[counter]).id;
			//(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).position.text = ((Challenger)challengers[counter]).position.ToString();
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).name.text = ((Challenger)challengers[counter]).displayName;
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).topScore.text = ((Challenger)challengers[counter]).topScore.ToString();
		}
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
using UnityEngine;
using System.Collections;

public class CheckFundsForChallenge : MonoBehaviour {
	
	public GameSparks GSApi;
	
	public UIButton BuyCoinsButton;
	public UIButton AcceptButton;
	public UIButton PlayButton;
	public UIButton RejectButton;
	
	// Use this for initialization
	void Start () {
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;

		Hashtable accountDetails = GSApi.accountDetails();
		Hashtable challengeDetails = GSApi.listChallengeDetails(GameGlobals.relevantID);
		double pot = (double)((Hashtable)challengeDetails["challenge"])["currency3Wager"];
		
		if(pot > ((double)accountDetails["currency3"])){
			AcceptButton.gameObject.SetActive(false);
			PlayButton.gameObject.SetActive(false);
			RejectButton.transform.localPosition = new Vector3(400f,-1275.456f,0f);
		}
		else{
			BuyCoinsButton.gameObject.SetActive(false);
		}
		
		
		
		

		
		/*
		if(false){
			AcceptButton.gameObject.SetActive(false);
			PlayButton.gameObject.SetActive(false);
			RejectButton.transform.localPosition = new Vector3(400f,-1275.456f,0f);
		}
		else{
			BuyCoinsButton.gameObject.SetActive(false);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

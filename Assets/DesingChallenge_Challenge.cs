using UnityEngine;
using System.Collections;
using System;


public class DesingChallenge_Challenge : MonoBehaviour {
	public GameSparks GSApi;
	public UIPopupList track;
	public UIPopupList attempts;
	public UIPopupList duration;
	public UIPopupList stakes;
	public UIInput textBox;
	
	
	// Use this for initialization
	void Start () {
		if(GameGlobals.selectedTrack.dlc && !GameGlobals.selectedTrack.purchased){
			GameGlobals.selectedTrack = ( (AudioTrack) GameGlobals.Songs["Clu - Ruby"] );
		}
		
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		
		Hashtable accountDetails = GSApi.accountDetails();
		if(accountDetails != null)
		{
			
			int currency  =(int)((double)accountDetails["currency3"]);
			stakes.items.Clear();
			
			for(int i = 0; i <= currency && i < 1000; i+=100)
			{
				stakes.items.Add(i.ToString());
			}
			stakes.selection = "0";
			
			track.items.Clear();
			foreach(DictionaryEntry currentTrack in GameGlobals.Songs){
				
				//Check to see if this track is dlc & bought or not DLC before adding it as an option
				if( !((AudioTrack)currentTrack.Value).dlc || ((AudioTrack)currentTrack.Value).purchased )
					track.items.Add((string)currentTrack.Key);
				//Pierce/ it would be nice if we could see if other people in this challenge own this song before adding it too
				//or I could make another scene that says you need to purchase "song" to play this challenge hmmm, sounds like fucking work to me
				
				if( GameGlobals.selectedTrack.song.Contains((string)currentTrack.Key) )
					track.selection = (string)currentTrack.Key;
			}
		}
	}
	void OnClick()
	{
		string[] IDs = new string[GameGlobals.otherIDs.Count];
		for(int i=0; i < GameGlobals.otherIDs.Count; i++)
		{
			IDs[i] = (string)(GameGlobals.otherIDs[i]);
		}
		
		GameGlobals.relevantID = ( (string)( (Hashtable) GSApi.createChallenge(selectedTrackSC(), GetMessage(),Wager(), EndDate(), Attempts(), IDs) )["challengeInstanceId"] );
		//GameGlobals.shortCode = selectedTrackSC();
		GameGlobals.loading = true;
		Application.LoadLevelAsync("ChallengeDetails");
		
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	string selectedTrackSC()
	{
		return ((AudioTrack)GameGlobals.Songs[track.selection]).challengeShortCode;
		
		
	}
	int Wager()
	{
		return int.Parse((string)stakes.selection);
	}
	string GetMessage()
	{
		return textBox.text;
	}
	int Attempts()
	{
		return int.Parse(attempts.selection);
	}
	DateTime EndDate()
	{
		switch (duration.selection) {
		case "1 Day":
			return DateTime.Now.AddDays(1);
		case "2 Days":
			return DateTime.Now.AddDays(2);
		case "3 Days":
			return DateTime.Now.AddDays(3);
		case "4 Days":
			return DateTime.Now.AddDays(4);
		case "5 Days":
			return DateTime.Now.AddDays(5);
		case "6 Days":
			return DateTime.Now.AddDays(6);
		case "1 Week":
			return DateTime.Now.AddDays(7);
		default:
		break;
		}
		return DateTime.Today.AddDays(7);
	}
	
}

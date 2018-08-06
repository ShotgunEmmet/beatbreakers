using UnityEngine;
using System.Collections;

public class Scroll_Item_ChallengesList : MonoBehaviour {
	
	public string id;
	public UILabel name;
	public UILabel pot;
	public UILabel status;
	public UILabel ends;
	
	public static int itemCount = 1;
	
	void Start()
	{
		transform.parent.name = itemCount.ToString("00");
		itemCount++;
	}
	
	public void Highlight(){
		if(status.text.ToLower() == "waiting")
			status.text = "[00ff00]"+ status.text;
		else if(status.text.ToLower() == "recieved")
			status.text = "[ffff00]"+ status.text;
		else if(status.text.ToLower() == "complete")
			status.text = "[ff0000]"+ status.text;
			
	}
	
	void OnClick()
	{
		//load Challenge
		//set a global value to the challenge id & read it in the start function of ChallengeDetails
		
		GameGlobals.relevantID = id;
		 Debug.Log(status.text.ToString().ToLower());
		if(status.text.ToString().Substring(8).ToLower() == "waiting"){
			GameGlobals.loading = true;
			Application.LoadLevelAsync("ChallengeDetails");
		}
		else if(status.text.ToString().Substring(8).ToLower() == "recieved"){
			GameGlobals.loading = true;
			Application.LoadLevelAsync("UnacceptedChallengeDetails");
		}
		else if(status.text.ToString().Substring(8).ToLower() == "complete"){
			GameGlobals.loading = true;
			Application.LoadLevelAsync("CompletedChallengeDetails");
		}
			
	}
		
}

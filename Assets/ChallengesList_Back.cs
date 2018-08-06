using UnityEngine;
using System.Collections;

public class ChallengesList_Back : MonoBehaviour {

	
	void OnClick()
	{
		//load FriendsList
		GameGlobals.loading = true;
		Application.LoadLevelAsync("FriendsList");
	}
}

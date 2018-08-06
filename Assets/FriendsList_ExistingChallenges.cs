using UnityEngine;
using System.Collections;

public class FriendsList_ExistingChallenges : MonoBehaviour {

	void OnClick()
	{
		//load ChallengesList
		GameGlobals.loading = true;
		Application.LoadLevelAsync("ChallengesList");
	}
}

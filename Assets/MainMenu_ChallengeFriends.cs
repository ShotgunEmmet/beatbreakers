using UnityEngine;
using System.Collections;

public class MainMenu_ChallengeFriends : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("FriendsList");
	}
}

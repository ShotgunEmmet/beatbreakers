using UnityEngine;
using System.Collections;

public class DesignChallenge_XButton : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("FriendsList");
	}
}

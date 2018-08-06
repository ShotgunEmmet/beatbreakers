using UnityEngine;
using System.Collections;

public class Challenge_XButton : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("ChallengesList");
	}
	
}

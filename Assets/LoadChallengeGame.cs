using UnityEngine;
using System.Collections;

public class LoadChallengeGame : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		//Application.LoadLevelAsync("AgainstTheClock");
		Application.LoadLevelAsync("BuyTracks");
	}
}

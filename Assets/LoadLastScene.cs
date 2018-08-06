using UnityEngine;
using System.Collections;

public class LoadLastScene : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		if(GameGlobals.lastScene == "Game")
			Application.LoadLevelAsync("BuyTracks");
		else
			Application.LoadLevelAsync(GameGlobals.lastScene);
	}
}

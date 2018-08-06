using UnityEngine;
using System.Collections;

public class LoadSinglePlayerGame : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		//Application.LoadLevelAsync("BuyTracks");
		Application.LoadLevelAsync("Journey");
	}
}

using UnityEngine;
using System.Collections;

public class PlayAgainstTheClock : MonoBehaviour {

	
	void OnClick(){
		GameGlobals.gameState = GameGlobals.GameState.AgainstTheClock;
		GameGlobals.loading = true;
		MenuMusic.Instance.SetEnabled(false);
		Application.LoadLevelAsync("Game");
	}
	
}

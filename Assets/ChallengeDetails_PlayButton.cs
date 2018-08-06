using UnityEngine;
using System.Collections;

public class ChallengeDetails_PlayButton : MonoBehaviour {

	void OnClick(){

		GameGlobals.gameState = GameGlobals.GameState.Challenge;
		GameGlobals.loading = true;
		MenuMusic.Instance.SetEnabled(false);
		Application.LoadLevelAsync("Game");
	}
}

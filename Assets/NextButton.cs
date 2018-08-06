using UnityEngine;
using System.Collections;

public class NextButton : MonoBehaviour {

	public EndScreen endScreen;
	public GameObject nextBackground, returnBackground;
	private int oldScreen = 0;
	
	void OnClick(){
		endScreen.Next();
		if(GameGlobals.gameState == GameGlobals.GameState.Journey){
			if(oldScreen != endScreen.screen){
				oldScreen = endScreen.screen;
				nextBackground.SetActive(false);
				returnBackground.SetActive(true);
			}
		}
	}
	
	void Update(){
	}
}

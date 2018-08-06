using UnityEngine;
using System.Collections;

public class UseMultiplier : MonoBehaviour {
	
	public int multiplierNumber = 0;
	private UILabel label;
	
	private bool active;
	
	// Use this for initialization
	void Start()
	{
		GameGlobals.MultiplierDuration = 0;
		
		label = (UILabel)GetComponent(typeof(UILabel));
		
		if(GameGlobals.gameState == GameGlobals.GameState.Challenge){//GameGlobals.opponentsScore >= 0){
			gameObject.SetActive(false);
		}
		else if(PlayerPrefs.GetInt("X2s") > multiplierNumber){
			label.color = new Color(124f/255f,124f/255f,124f/255f,1);
			active = true;
		}
		else{
			label.color = new Color(40f/255f,40f/255f,40f/255f,1);
			active = false;
		}
	}
	
	// Update is called once per frame
	void OnClick () {
		if(active){
			if(GameGlobals.MultiplierDuration <= 0){
				active = false;
				PlayerPrefs.SetInt("X2s",PlayerPrefs.GetInt("X2s")-1);
				label.color = new Color(40f/255f,40f/255f,40f/255f,1);//124 40
				GameGlobals.MultiplierDuration = 10f;
			}
		}
	}
}

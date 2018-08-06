using UnityEngine;
using System.Collections;

public class GamePlayButton : MonoBehaviour {
	
	public GameObject image;
	public GameObject tinyImage;
	public GameObject pauseBackground;
	public GameObject retryButton;
	public GameObject retryImage;
	public GameObject homeButton;
	public GameObject homeImage;
	public No noButton;
	
	void Start(){
		GameGlobals.gamePaused = false;
		transform.collider.enabled = false;
		image.SetActive(GameGlobals.gamePaused);
		tinyImage.SetActive(GameGlobals.gamePaused);
		pauseBackground.SetActive(GameGlobals.gamePaused);
		retryButton.SetActive(false);
		retryButton.collider.enabled = false;
		retryImage.SetActive(false);
		homeButton.SetActive(false);
		homeButton.collider.enabled = false;
		homeImage.SetActive(false);
		noButton.OnClick();
	}
	
	void Update(){
		if(GameGlobals.gamePaused){
			if(!image.activeSelf){
				transform.collider.enabled = true;
				image.SetActive(true);	
				tinyImage.SetActive(true);	
				pauseBackground.SetActive(true);
				if(GameGlobals.gameState == GameGlobals.GameState.Challenge){
					
					//if the number of attempts is above 0 (accounting for this one)					
					if(GameGlobals.challengeAttemptsRemaining > 1){
						retryButton.SetActive(true);
						retryButton.collider.enabled = true;
						retryImage.SetActive(true);
					}
					else{
						homeButton.gameObject.transform.localPosition = new Vector3(-135f ,homeButton.gameObject.transform.localPosition.y, homeButton.gameObject.transform.localPosition.z);
					}
				}
				else{
					retryButton.SetActive(true);
					retryButton.collider.enabled = true;
					retryImage.SetActive(true);
				}
				homeButton.SetActive(true);
				homeButton.collider.enabled = true;
				homeImage.SetActive(true);
			}
		}
	}
	
	void OnClick(){
		if(GameGlobals.gamePaused){
			if(image.activeSelf){
				 Debug.Log(retryButton.activeSelf.ToString());
				GameGlobals.gamePaused = false;
				transform.collider.enabled = false;
				image.SetActive(false);	
				tinyImage.SetActive(false);	
				pauseBackground.SetActive(false);
				retryButton.SetActive(false);
				retryButton.collider.enabled = false;
				retryImage.SetActive(false);
				homeButton.SetActive(false);
				homeButton.collider.enabled = false;
				homeImage.SetActive(false);
				noButton.OnClick();
			}
		}
	}
	
}

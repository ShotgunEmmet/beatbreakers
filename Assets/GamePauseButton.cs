using UnityEngine;
using System.Collections;

public class GamePauseButton : MonoBehaviour {

	public GameObject image;
	public GameObject tinyImage;
	public GameObject retryButton;
	public GameObject retryImage;
	public GameObject homeButton;
	public GameObject homeImage;
	
	
	void Update(){
		if(!GameGlobals.gamePaused){
			if(!image.activeSelf){
				transform.collider.enabled = true;
				image.SetActive(true);
				tinyImage.SetActive(true);
			}
		}
	}
	
	void OnClick(){
		if(!GameGlobals.gamePaused){
			if(image.activeSelf){
				GameGlobals.gamePaused = true;
				transform.collider.enabled = false;
				image.SetActive(false);
				tinyImage.SetActive(false);
			}
		}
	}
	
}

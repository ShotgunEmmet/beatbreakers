using UnityEngine;
using System.Collections;

public class JourneyLoadGame : MonoBehaviour {
	
	public string trackName;
	public Renderer frame;
	public JourneyLoadGame previousTrack, nextTrack;
	public Texture2D locked,unlocked,bronze,silver,gold;
	
	void Start(){
		if(previousTrack != null && PlayerPrefs.GetInt(previousTrack.trackName+"unlocked") == 0){
			transform.collider.enabled = false;
			//show lock
			frame.material.mainTexture = locked;
		}
		else if(PlayerPrefs.GetInt(trackName+"unlocked") == 0){
			(GetComponent(typeof(TweenScale)) as TweenScale).enabled = true;
			
			frame.material.mainTexture = unlocked;
		}
		
		 Debug.Log(trackName + " = " + PlayerPrefs.GetInt(trackName+"unlocked"));
		if(PlayerPrefs.GetInt(trackName+"unlocked") == 1){
			//show bronze frame
			frame.material.mainTexture = bronze;
		}
		else if(PlayerPrefs.GetInt(trackName+"unlocked") == 2){
			//show silver frame
			frame.material.mainTexture = silver;
		}
		else if(PlayerPrefs.GetInt(trackName+"unlocked") == 3){
			//show gold frame
			frame.material.mainTexture = gold;
		}
	}
	
	void OnClick(){
		if(trackName == "")
			trackName = "Clu - Ruby";
		
		//just in case your song isn't in the list
		//GameGlobals.selectedTrack = (AudioTrack)GameGlobals.Songs[0];
		/*
		foreach(AudioTrack currentTrack in GameGlobals.Songs){
			if(currentTrack.song.Contains(trackName)){
				GameGlobals.selectedTrack = currentTrack;
				break;
			}
		}
		*/
		GameGlobals.selectedTrack = (AudioTrack)GameGlobals.Songs[trackName];
		GameGlobals.gameState = GameGlobals.GameState.Journey;
		GameGlobals.loading = true;
		MenuMusic.Instance.SetEnabled(false);
		Application.LoadLevelAsync("Game");
	}
}

using UnityEngine;
using System.Collections;

public class TrackStoreItem : MonoBehaviour {
	
	public Renderer albumArt;
	public UILabel label;
	public Renderer locked;
	
	private GameObject buySection;
	private GameObject selectTrackSections;
	
	public GameObject BBCoin, GSCoin;
	
	private AudioTrack track;
	private string songName;
	DLCManager dlc;
	void Start(){
		foreach(Transform current in transform.parent.parent.parent.GetComponentsInChildren(typeof(Transform),true)){
			if(current.name == "BuySection")
				buySection = (GameObject)current.gameObject;
			else if(current.name == "Panel1")
				selectTrackSections = (GameObject)current.gameObject;
		}
	}
	
	public void SetupTrack(AudioTrack track){
		dlc = GameObject.FindObjectOfType(typeof(DLCManager)) as DLCManager;
		this.track = track;
		string[] songString = track.song.Split('/');
		string[] nameString = songString[songString.Length-1].Split('-');
		label.text = " "+nameString[0]+"\n"+nameString[1];
		
		if(track.dlc){
			 Debug.Log("dlc images");
			albumArt.material.mainTexture  =  (Texture2D)dlc.DLCBundle.Load(track.albumArt.Replace(".png", ""));
			
		}
		else
			albumArt.material.mainTexture = Resources.Load(track.albumArt, typeof(Texture2D)) as Texture2D;
		
		this.songName = songString[songString.Length-1];
		
		if( (track.dlc && track.purchased) ||  PlayerPrefs.GetInt(this.songName+"unlocked") > 0){
			locked.enabled = false;
			//buyTrack.enabled = false;
			BBCoin.SetActive(false);
			GSCoin.SetActive(false);
		}
		else if(track.dlc){
			//locked.enabled = false;
			if(track.inGameCurrencyCost < 0){
				//show in game currency cost & button
				BBCoin.SetActive(false);
			}
			//shoe gamespark currency cost & button
		}
		else{
			//buyTrack.enabled = false;
			BBCoin.SetActive(false);
			GSCoin.SetActive(false);
		}
		
	}
	
	
	void OnClick(){
		GameGlobals.selectedTrack = track;
		if( (track.dlc && track.purchased) ||  PlayerPrefs.GetInt(this.songName+"unlocked") > 0)
		{
			//Pierce/ requires a check to see if the song is downloaded before loading (if bought)
			GameGlobals.opponentsScore = -1;
			GameGlobals.gameState = GameGlobals.GameState.AgainstTheClock;
			GameGlobals.loading = true;
			MenuMusic.Instance.SetEnabled(false);
			Application.LoadLevelAsync("Game");
		}
		else if(track.dlc){
			//( (UILabel) GameObject.Find("MessageLabel").GetComponent(typeof(UILabel)) ).text = "Buy this track\nto unlock it";
			
			buySection.SetActive(true);
			foreach(Transform current in buySection.GetComponentsInChildren(typeof(Transform),true)){
				if(current.name == "TrackTitle"){
					string[] songString = GameGlobals.selectedTrack.song.Split('/');
					string[] nameString = songString[songString.Length-1].Split('-');
					( (UILabel) ((GameObject)current.gameObject).GetComponent(typeof(UILabel)) ).text = nameString[0]+"\n"+nameString[1]+" ";
				}
				else if(current.name == "SpendInGameCurrencyButton"){
					if(track.inGameCurrencyCost < 0){
						current.gameObject.SetActive(false);
					}
					else{
						( (UILabel) ((GameObject)current.gameObject).GetComponentInChildren(typeof(UILabel)) ).text = GameGlobals.selectedTrack.inGameCurrencyCost.ToString();
					}
				}
				else if(current.name == "SpendRealMoneyButton"){
					( (UILabel) ((GameObject)current.gameObject).GetComponentInChildren(typeof(UILabel)) ).text = GameGlobals.selectedTrack.gamesparkCurrencyCost.ToString();
				}
			}
			selectTrackSections.SetActive(false);
			transform.parent.parent.gameObject.SetActive(false);
		}
		else{
			( (UILabel) GameObject.Find("MessageLabel").GetComponent(typeof(UILabel)) ).text = "This track has not been\nunlocked in Journey mode";
		}
	}
	
}

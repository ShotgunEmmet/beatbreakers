using UnityEngine;
using System.Collections;

public class PopulateTracks : MonoBehaviour {
	
	UIPopupList popupMenu;
	string oldValue;
	
	public PopulateHighscoreTable populator;
	
	public GameObject playButton;
	
	// Use this for initialization
	void Start () {
		popupMenu = ( (UIPopupList) GetComponent(typeof(UIPopupList)) );
		popupMenu.items.Clear();
		foreach(DictionaryEntry track in GameGlobals.Songs){
			string[] song = ((AudioTrack)track.Value).song.ToString().Split('/');
			
			popupMenu.items.Add(song[song.Length-1]);
			
			if( GameGlobals.selectedTrack.song.Contains(song[song.Length-1]) )
				popupMenu.selection = song[song.Length-1];
		}
		
		oldValue = popupMenu.selection;
		
		/*
		popupMenu = ( (UIPopupList) GetComponent(typeof(UIPopupList)) );
		popupMenu.items.Clear();
		foreach(AudioTrack track in GameGlobals.Songs){
			string[] song = track.song.Split('/');
			//string[] art = track.albumArt.Split('/');
			popupMenu.items.Add(song[song.Length-1]);
			
			if( GameGlobals.selectedTrack.song.Contains(song[song.Length-1]) )
				popupMenu.selection = song[song.Length-1];
		}
		
		oldValue = popupMenu.selection;
		*/
		
		if(GameGlobals.selectedTrack.dlc && !GameGlobals.selectedTrack.purchased)
			playButton.SetActive(false);
		
		populator.PopulateTable(oldValue);
		
	}
	
	void Update(){
		if(oldValue != popupMenu.selection){
			oldValue = popupMenu.selection;
			
			foreach(DictionaryEntry track in GameGlobals.Songs){
				string[] song = ((AudioTrack)track.Value).song.Split('/');
				
				if( song[song.Length-1].Contains(oldValue) ){
					GameGlobals.selectedTrack = ((AudioTrack)track.Value);
					populator.PopulateTable(oldValue);
					if(GameGlobals.selectedTrack.dlc && !GameGlobals.selectedTrack.purchased)
						playButton.SetActive(false);
					else
						playButton.SetActive(true);
				}
			}
			
			/*
			foreach(AudioTrack track in GameGlobals.Songs){
				if( track.song.Contains(popupMenu.selection) ){
					GameGlobals.selectedTrack = track;
					populator.PopulateTable(oldValue);
				}
			}
			*/
			
		}
		
	}
}

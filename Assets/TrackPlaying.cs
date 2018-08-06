using UnityEngine;
using System.Collections;

public class TrackPlaying : MonoBehaviour {
	
	void Start () {
		string[] songString = GameGlobals.selectedTrack.song.Split('/');
		string[] nameString = songString[songString.Length-1].Split('-');
		((UILabel)GetComponent(typeof(UILabel))).text = nameString[0]+"\n"+nameString[1]+" ";
		
		
	}
	
}

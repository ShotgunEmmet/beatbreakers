using UnityEngine;
using System.Collections;

public class No : MonoBehaviour {
	
	public ReplayButton replay;
	public HomeButton home;
	
	public void OnClick(){
		if(replay.clicked){
			replay.Hide();
		}
		else if(home.clicked){
			home.Hide();
		}
	}
}

using UnityEngine;
using System.Collections;

public class Yes : MonoBehaviour {

	public ReplayButton replay;
	public HomeButton home;
	
	void OnClick(){
		if(replay.clicked){
			replay.Load();
		}
		else if(home.clicked){
			home.Load();
		}
	}
	
}

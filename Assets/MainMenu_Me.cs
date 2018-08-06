using UnityEngine;
using System.Collections;

public class MainMenu_Me : MonoBehaviour {
	
	void Update(){
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Alpha0))//add touch code here later :P
		{
			PlayerPrefs.SetInt("Clu - Ruby"+"unlocked", 0);
			PlayerPrefs.SetInt("Clu + Ft. Lindstrøm - Rà-àkõ-st"+"unlocked", 0);
			PlayerPrefs.SetInt("Cosmic Boy - Survival"+"unlocked", 0);
			PlayerPrefs.SetInt("Knife City - Bad News"+"unlocked", 0);
			PlayerPrefs.SetInt("Knife City - Just Trash"+"unlocked", 0);
			PlayerPrefs.SetInt("Polygon APE - IncrediBULL"+"unlocked", 0);
			PlayerPrefs.SetInt("Polygon APE - Riff Raff"+"unlocked", 0);
			PlayerPrefs.SetInt("RIOT AKKT - TIGER CHILD"+"unlocked", 0);
			PlayerPrefs.SetInt("Sabrepulse + Knife City - First Crush"+"unlocked", 0);
			PlayerPrefs.SetInt("Sabrepulse - A Girl I Know"+"unlocked", 0);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha1)){
			PlayerPrefs.SetInt("Clu - Ruby"+"unlocked", 1);
			PlayerPrefs.SetInt("Clu + Ft. Lindstrøm - Rà-àkõ-st"+"unlocked", 1);
			PlayerPrefs.SetInt("Cosmic Boy - Survival"+"unlocked", 1);
			PlayerPrefs.SetInt("Knife City - Bad News"+"unlocked", 1);
			PlayerPrefs.SetInt("Knife City - Just Trash"+"unlocked", 1);
			PlayerPrefs.SetInt("Polygon APE - IncrediBULL"+"unlocked", 1);
			PlayerPrefs.SetInt("Polygon APE - Riff Raff"+"unlocked", 1);
			PlayerPrefs.SetInt("RIOT AKKT - TIGER CHILD"+"unlocked", 1);
			PlayerPrefs.SetInt("Sabrepulse + Knife City - First Crush"+"unlocked", 1);
			PlayerPrefs.SetInt("Sabrepulse - A Girl I Know"+"unlocked", 1);
		}
		
#endif
	}
	
	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("Me");
	}
	
}

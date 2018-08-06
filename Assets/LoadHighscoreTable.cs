using UnityEngine;
using System.Collections;

public class LoadHighscoreTable : MonoBehaviour {

	void OnClick(){
		GameGlobals.lastScene = Application.loadedLevelName;
		GameGlobals.loading = true;
		Application.LoadLevelAsync("HighscoreTable");
	}
}

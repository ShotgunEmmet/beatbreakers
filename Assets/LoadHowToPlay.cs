using UnityEngine;
using System.Collections;

public class LoadHowToPlay : MonoBehaviour {

	void OnClick(){
		GameGlobals.lastScene = Application.loadedLevelName;
		GameGlobals.loading = true;
		Application.LoadLevelAsync("HowToPlay");
	}
}

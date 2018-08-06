using UnityEngine;
using System.Collections;

public class LoadMainMenu : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("MainMenuNGUI");
	}
}

using UnityEngine;
using System.Collections;

public class LoadSettingsScene : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("Settings");
	}
}

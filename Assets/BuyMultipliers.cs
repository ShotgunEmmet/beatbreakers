using UnityEngine;
using System.Collections;

public class BuyMultipliers : MonoBehaviour {

	void OnClick(){
		GameGlobals.lastScene = Application.loadedLevelName;
		GameGlobals.loading = true;
		Application.LoadLevelAsync("BuyMultipliers");
	}
	
}

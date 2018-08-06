using UnityEngine;
using System.Collections;

public class AddCoins : MonoBehaviour {

	void OnClick(){
		GameGlobals.lastScene = Application.loadedLevelName;
		GameGlobals.loading = true;
		Application.LoadLevelAsync("BuyCoins");
	}
	
}

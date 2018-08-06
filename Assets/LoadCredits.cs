using UnityEngine;
using System.Collections;

public class LoadCredits : MonoBehaviour {

	void OnClick(){
		GameGlobals.loading = true;
		Application.LoadLevelAsync("Credits");
	}
}

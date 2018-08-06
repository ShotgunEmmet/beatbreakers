using UnityEngine;
using System.Collections;

public class DeleteCompletedChallenge : MonoBehaviour {

	void OnClick(){
		//Pierce/ Delete this from the list of challenges I can see
		
		GameGlobals.loading = true;
		Application.LoadLevelAsync("ChallengesList");
	}
}

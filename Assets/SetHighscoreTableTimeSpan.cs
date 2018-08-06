using UnityEngine;
using System.Collections;

public class SetHighscoreTableTimeSpan : MonoBehaviour {
	
	public PopulateHighscoreTable highscoreTable;
	public PopulateHighscoreTable.TimeSpan timeSpan;
	
	void OnClick(){
		highscoreTable.timeSpan = timeSpan;
	}
	
}

using UnityEngine;
using System.Collections;

public class PopulateHighscoreTable : MonoBehaviour {

	public GameObject playerScorePrefab;
	public GameSparks GSApi;
	ArrayList playerScores;
	public UILabel label;
	public enum TimeSpan {overall = 0, month, week, day};
	public TimeSpan timeSpan = TimeSpan.overall;
	
	// Use this for initialization
	void Start () {
		
		
		
		
		
		
		
		

		
		PopulateTable(GameGlobals.selectedTrack.song);
	}
	
	
	public void PopulateTable(string track){
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		Hashtable leaderHash = GSApi.Leaderboards(((AudioTrack)GameGlobals.Songs[label.text]).challengeShortCode);
		playerScores = new ArrayList();
		ArrayList pScores = (ArrayList)leaderHash["data"];
		
		
		foreach(Hashtable pScore in pScores)
		{
			int score = 0;
			int attemptCount = 0; 
			
			 Debug.Log("adding player");
			
			playerScores.Add(new Challenger{id = (string)pScore["userId"], displayName = (string)pScore["userName"], topScore = (int)(double)pScore["HS"],attempts= attemptCount});
			
		}
		
		

		for(int counter = transform.childCount-1; counter >= 0; counter-- ){
			Destroy(transform.GetChild(counter).gameObject);
		}
		( (UIScrollBar)GameObject.FindObjectOfType(typeof(UIScrollBar)) ).scrollValue = 0;
		
		Scroll_Item_ChallengesList.itemCount=1;
		 Debug.Log(playerScores.Count);
		for(int counter = 0; counter < playerScores.Count; counter++){
			GameObject item = Instantiate(playerScorePrefab,playerScorePrefab.transform.position,Quaternion.identity) as GameObject;
			item.transform.parent = this.transform;
			item.transform.localScale = new Vector3(1,1,1);
			item.transform.localPosition = item.transform.position + new Vector3(0,-46*counter,0);
			 Debug.Log(item.GetType());
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).id = ((Challenger)playerScores[counter]).id;
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).name.text = ((Challenger)playerScores[counter]).displayName;
			(item.GetComponentInChildren(typeof(Scroll_Item_Challengers_Finished)) as Scroll_Item_Challengers_Finished).topScore.text = ((Challenger)playerScores[counter]).topScore.ToString();
			
		}
	}
}

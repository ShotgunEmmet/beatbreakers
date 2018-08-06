using UnityEngine;
using System.Collections;

public class PopulateChallengesList : MonoBehaviour {

	public GameObject challengePrefab;
	public GameSparks GSApi;
	ArrayList challenges;
	
	
	// Use this for initialization
	void Start () {
		
		challenges = new ArrayList();
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		Hashtable challengesHash = GSApi.listRunningChallenges();
		
		ArrayList challengeInstances = challengesHash["challengeInstances"] as ArrayList;
		foreach(Hashtable challenge in challengeInstances)
		{
			Challenge c= new Challenge();
			c.id=(string)challenge["challengeId"];
			c.displayName = (string)(((Hashtable)challenge["challenger"])["name"]) ;
			c.pot = challenge["currency3Wager"] == null? 0: (double)challenge["currency3Wager"];
			
			c.status = Challenge.ChallengeStatus.waiting;
			c.ends = (string)challenge["endDate"];
			challenges.Add(c);
			 Debug.Log(challenge["endDate"]);
		}
		challengesHash = GSApi.listRecievedChallenges();
		
		challengeInstances = challengesHash["challengeInstances"] as ArrayList;
		if(challengeInstances != null)
			foreach(Hashtable challenge in challengeInstances)
			{
				Challenge c= new Challenge();
				c.id=(string)challenge["challengeId"];
				c.displayName = (string)(((Hashtable)challenge["challenger"])["name"]) ;
				c.pot = challenge["currency3Wager"] == null? 0: (double)challenge["currency3Wager"];
				
				c.status = Challenge.ChallengeStatus.recieved;
				c.ends = (string)challenge["endDate"];
				challenges.Add(c);
			
			}
		challengesHash = GSApi.listCompletedChallenges();
		
		challengeInstances = challengesHash["challengeInstances"] as ArrayList;
		if(challengeInstances != null)
		foreach(Hashtable challenge in challengeInstances)
		{
			Challenge c= new Challenge();
			c.id=(string)challenge["challengeId"];
			c.displayName = (string)(((Hashtable)challenge["challenger"])["name"]) ;
			c.pot = challenge["currency3Wager"] == null? 0: (double)challenge["currency3Wager"];
				
			c.status = Challenge.ChallengeStatus.complete;
			c.ends = (string)challenge["endDate"];
			challenges.Add(c);
			
		}
		/*
		
		challenges = new ArrayList();
		//populate from gamespark
		challenges.Add(new Challenge{id = "a", displayName = "Alex", pot = 100, status = Challenge.ChallengeStatus.waiting, ends = 0});
		challenges.Add(new Challenge{id = "b", displayName = "Billy", pot = 200, status = Challenge.ChallengeStatus.waiting, ends = 1});
		challenges.Add(new Challenge{id = "c", displayName = "Claire", pot = 300, status = Challenge.ChallengeStatus.waiting, ends = 2});
		challenges.Add(new Challenge{id = "d", displayName = "Derick", pot = 400, status = Challenge.ChallengeStatus.waiting, ends = 3});
		challenges.Add(new Challenge{id = "e", displayName = "Emmet", pot = 500, status = Challenge.ChallengeStatus.waiting, ends = 25});
		challenges.Add(new Challenge{id = "f", displayName = "Frank", pot = 600, status = Challenge.ChallengeStatus.waiting, ends = 49});
		challenges.Add(new Challenge{id = "g", displayName = "Gregory", pot = 700, status = Challenge.ChallengeStatus.waiting, ends = 73});
		*/
		
		//PopulateList("waiting");
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void PopulateList(string type){
		for(int counter = transform.childCount-1; counter >= 0; counter-- ){
			Destroy(transform.GetChild(counter).gameObject);
		}
		( (UIScrollBar)GameObject.FindObjectOfType(typeof(UIScrollBar)) ).scrollValue = 0;
		
		Scroll_Item_ChallengesList.itemCount=1;
		int entryCount = 0;
		for(int counter = 0; counter < challenges.Count; counter++){
			// Debug.LogError(((Challenge)challenges[counter]).status.ToString());
			if(type.ToLower() == ((Challenge)challenges[counter]).status.ToString().ToLower() || type.ToLower() == "all"){
				
				GameObject item = Instantiate(challengePrefab,challengePrefab.transform.position,Quaternion.identity) as GameObject;
				item.transform.parent = this.transform;
				item.transform.localScale = new Vector3(1,1,1);
				item.transform.localPosition = item.transform.position + new Vector3(0,-46*entryCount,0);
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).id = ((Challenge)challenges[counter]).id;
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).name.text = ((Challenge)challenges[counter]).displayName;
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).pot.text = ((Challenge)challenges[counter]).pot.ToString();
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).status.text = ((Challenge)challenges[counter]).status.ToString();
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).ends.text = TimeRemaining(((Challenge)challenges[counter]).ends);
				
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).Highlight();
				
				entryCount++;
			}
			else if( type.ToLower().Substring(8) ==((Challenge)challenges[counter]).status.ToString().ToLower() ){
				
				GameObject item = Instantiate(challengePrefab,challengePrefab.transform.position,Quaternion.identity) as GameObject;
				item.transform.parent = this.transform;
				item.transform.localScale = new Vector3(1,1,1);
				item.transform.localPosition = item.transform.position + new Vector3(0,-46*entryCount,0);
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).id = ((Challenge)challenges[counter]).id;
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).name.text = ((Challenge)challenges[counter]).displayName;
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).pot.text = ((Challenge)challenges[counter]).pot.ToString();
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).status.text = ((Challenge)challenges[counter]).status.ToString();
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).ends.text = TimeRemaining(((Challenge)challenges[counter]).ends);
				
				(item.GetComponentInChildren(typeof(Scroll_Item_ChallengesList)) as Scroll_Item_ChallengesList).Highlight();
				
				entryCount++;
			}
			
		}
	}
	
	long DaysFromMonths(string month, string year){
		long returnValue = 0;
		//account for months (switch statement won't work well here)
		if(long.Parse(month) > 1)//jan
			returnValue += 31;
		if(long.Parse(month) > 2)//feb
			if(long.Parse(year)%4 == 0)
				returnValue += 29;
			else
				returnValue += 28;
		if(long.Parse(month) > 3)//mar
			returnValue += 31;
		if(long.Parse(month) > 4)//apr
			returnValue += 30;
		if(long.Parse(month) > 5)//may
			returnValue += 31;
		if(long.Parse(month) > 6)//jun
			returnValue += 30;
		if(long.Parse(month) > 7)//jul
			returnValue += 31;
		if(long.Parse(month) > 8)//aug
			returnValue += 31;
		if(long.Parse(month) > 9)//sep
			returnValue += 30;
		if(long.Parse(month) > 10)//oct
			returnValue += 31;
		if(long.Parse(month) > 11)//now
			returnValue += 30;
		
		return returnValue;
	}
	
	string TimeRemaining(string ends){
		// - - T : Z
		ends = ends.TrimEnd('Z');//remove the weird Z at the end of the date
		ends = ends.Replace(':','-');// change the : to a -
		ends = ends.Replace('T','-');// change the T to a -
		
		string[] date = ends.Split('-');//split up the time differences
		//year, month, day, hour, minute
		
		long endTime = long.Parse(date[0]) * 365;
		endTime += (long.Parse(date[0]) / 4);// I'm choosing to do the addative approach
		
		endTime += DaysFromMonths(date[1],date[0]);//days in all the months (dependant on leap years)
		
		endTime += long.Parse(date[2]);//days
		endTime *= 24;
		endTime += long.Parse(date[3]);//hours
		endTime *= 60;
		endTime += long.Parse(date[4]);//minutes
		//endTime *= 60;
		
		
		string now =System.DateTime.Now.ToString();
		now = now.Replace(' ','/');// change the : to a -
		now = now.Replace(':','/');// change the : to a -
		now = now.TrimEnd('A');
		now = now.TrimEnd('M');
		
		string[] date2 = now.Split('/');//split up the time differences
		//month, day, year, hour, minute
		long startTime = long.Parse(date2[2]) * 365;
		startTime += (long.Parse(date2[2]) / 4);// I'm choosing to do the addative approach
		
		startTime += DaysFromMonths(date2[0],date2[2]);//days in all the months (dependant on leap years)
		
		startTime += long.Parse(date2[1]);//days
		startTime *= 24;
		startTime += long.Parse(date2[3]);//hours
		startTime *= 60;
		startTime += long.Parse(date2[4]);//minutes
		
		endTime -= startTime;
		
		int minutes = (int)(endTime % 60);
		int hours = (int)((endTime / 60) % 24);
		int days = (int)((endTime / 60) / 24);
		
		// Debug.LogError(days+"-"+hours+"-"+minutes);
		
		
		if(endTime < 0)
			return "Done";
		else if(endTime < 60)
			return minutes.ToString() + " Min";
		else if(endTime < (24 * 60) )
			return hours.ToString() + " Hrs";
		else
			return days.ToString() + " Dys";
		
	}
}

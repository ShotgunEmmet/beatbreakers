using UnityEngine;
using System.Collections;

public class PopulateFriendsList : MonoBehaviour {
	
	public GameObject friendPrefab;
	
	ArrayList friends;
	public GameSparks GSApi;
	// Use this for initialization
	void Start () {
		GameGlobals.otherIDs.Clear();
		 Debug.Log("get gs");
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		friends = new ArrayList();
		Hashtable friendsGS =  GSApi.listGameFriends ();
		 Debug.Log("got gs");
		if(friendsGS!= null)
		{
			foreach(Hashtable friend in ((ArrayList)friendsGS["friends"]))
			{
				 Debug.Log("start adding friend");
				friends.Add(new Friend{id = (string)friend["id"], displayName = (string)friend["displayName"], online = (bool)friend["online"]});
				 Debug.Log("stop adding friend");
			}
		}
		else
		{
			 Debug.LogWarning("no friends found or not signed in");
		}
		/*foreach(DictionaryEntry friend in ((ArrayList)friendsGS["friends"]))
		{
			 Debug.Log(friend.Key);
			 Debug.Log(friend.Value);
		}*/
		//populate from gamespark
	/*	
		friends.Add(new Friend{id = "a", displayName = "aaa", online = true});
		friends.Add(new Friend{id = "b", displayName = "bbbbb", online = true});
		friends.Add(new Friend{id = "c", displayName = "ccccccccc", online = true});
		friends.Add(new Friend{id = "d", displayName = "dddd", online = true});
		friends.Add(new Friend{id = "e", displayName = "ee", online = true});
		friends.Add(new Friend{id = "f", displayName = "ffff", online = true});
		friends.Add(new Friend{id = "g", displayName = "ggggggg", online = true});
		*/
		Scroll_Item_FriendsList.itemCount=1;
		for(int counter = 0; counter < friends.Count; counter++){
			GameObject item = Instantiate(friendPrefab,friendPrefab.transform.position,Quaternion.identity) as GameObject;
			item.transform.parent = this.transform;
			item.transform.localScale = new Vector3(1,1,1);
			item.transform.localPosition = item.transform.position + new Vector3(0,-46*counter,0);
			(item.GetComponentInChildren(typeof(Scroll_Item_FriendsList)) as Scroll_Item_FriendsList).id = ((Friend)friends[counter]).id;
			(item.GetComponentInChildren(typeof(Scroll_Item_FriendsList)) as Scroll_Item_FriendsList).name.text = ((Friend)friends[counter]).displayName;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

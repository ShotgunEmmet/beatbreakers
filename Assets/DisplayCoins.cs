using UnityEngine;
using System.Collections;

public class DisplayCoins : MonoBehaviour {
	
	void Start(){
		//pierce/ if we're not online hide all this
		
		GameSparks GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		Hashtable accountDetails = GSApi.accountDetails();
		int currency  =(int)((double)accountDetails["currency3"]);
		
		(GetComponent(typeof(UILabel)) as UILabel).text = ((int)((double)accountDetails["currency3"])).ToString();
		//(GetComponent(typeof(UILabel)) as UILabel).text = coinPurse.ToString();
	}
}

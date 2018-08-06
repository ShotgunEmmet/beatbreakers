using UnityEngine;
using System.Collections;

public class DisplayInGameCoins : MonoBehaviour {

	// Use this for initialization
	void Start () {
		(GetComponent(typeof(UILabel)) as UILabel).text = PlayerPrefs.GetInt("InGameCurrency").ToString();
	}
	
}

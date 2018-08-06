using UnityEngine;
using System.Collections;

public class MultipliersRemaining : MonoBehaviour {

	// Use this for initialization
	void Start () {
		(GetComponent(typeof(UILabel)) as UILabel).text = "("+PlayerPrefs.GetInt("X2s").ToString()+" MULTIPLIERS REMAINING)";
	}
	
}

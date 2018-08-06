using UnityEngine;
using System.Collections;

public class ChallengesDropdown : MonoBehaviour {
	
	UIPopupList list;
	string oldOption;
	
	public PopulateChallengesList populator;
	
	// Use this for initialization
	void Start () {
		list = (UIPopupList)GetComponent(typeof(UIPopupList));
		oldOption = list.selection;
		populator.PopulateList(oldOption.ToLower());
	}
	
	// Update is called once per frame
	void Update () {
		if(oldOption != list.selection){
			oldOption = list.selection;
			populator.PopulateList(oldOption.ToLower());
		}
	}
}

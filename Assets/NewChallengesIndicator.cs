using UnityEngine;
using System.Collections;

public class NewChallengesIndicator : MonoBehaviour {
	
	private UILabel label;
	//pierce/ private int newChallenges;	//only needed if updating through Update()
	
	void Start(){
		label = (UILabel)GetComponent(typeof(UILabel));
		/*
		newChallenges = <number of unchecked challenges>
		if(newChallenges > 0)
			label.text = newChallenges.ToString();
		else
			label.text = "";
		*/
		label.text = "12";
	}
	
	/*
	void Update(){
		if(newChallenges != <number of unchecked challenges>){
			newChallenges = <number of unchecked challenges>
			label.text = newChallenges.ToString();
		}
	}
	*/
	
}

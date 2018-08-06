using UnityEngine;
using System.Collections;

public class DisableDropdown : MonoBehaviour {
	
	public Collider[] otherButtons;
	private UIButtonKeys thisButton;
	private bool thisButtonsInUse = false;
	
	// Use this for initialization
	void Start () {
		thisButton = (UIButtonKeys)GetComponent(typeof(UIButtonKeys));
	}
	
	// Update is called once per frame
	void Update () {
		if(!thisButton.enabled){
			thisButtonsInUse = true;
			foreach(Collider other in otherButtons){
				other.enabled = false;
			}
		}
		else{
			if(thisButtonsInUse){
				thisButtonsInUse = false;
				foreach(Collider other in otherButtons){
					other.enabled = true;
				}
			}
		}
	}
}

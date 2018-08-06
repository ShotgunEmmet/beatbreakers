using UnityEngine;
using System.Collections;

public class Resolution : MonoBehaviour {

	// Use this for initialization
	void Start () { 
		( (UILabel) GetComponent(typeof(UILabel)) ).text = "W: " + Screen.width + "\nH: " + Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class FacebookLoginButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick()
	{
#if !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN
		
		FacebookAndroid.login();
#endif
	}
}

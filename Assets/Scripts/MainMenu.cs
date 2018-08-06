using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using GameSparks;

#if UNITY_ANDROID
using FacebookAccess = FacebookAndroid;
#elif UNITY_IPHONE
using FacebookAccess = FacebookBinding;
#endif
public class MainMenu : MonoBehaviour {
	enum OnGUIState{ FB, Profile, Store, Friends, none
	};
	
	
	
	OnGUIState oNGUIState = OnGUIState.none;
	// Use this for initialization

	private int itemHeight = 30;
	private int itemWidth = 200;
	public GameObject beatLogo;
	
	//Declaring these static & readonly hides them in the IL code
	//private static readonly string apiKey = "22dTBsgAElpu";
	//private static readonly string apiSecret = "grsSZnyydqvCwFSs5iCk2hLP9q44GKqT";
	
	public GameSparks GSApi;
	
	
	
	void Awake ()
	{
		
		
		
	}


	void Start () {
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		

		
	}


	// Update is called once per frame
	void Update () {
		
	}

	
	
	Rect windowRect = new Rect(20, 20, Screen.width/2-20, Screen.height/2-20);
	void OnGUI ()
	{
		
		
		if( GUILayout.Button( "login" ) )
		{
			
#if UNITY_WEB_PLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
	
#else
			FacebookAccess.login();
#endif
			oNGUIState = OnGUIState.none;
		}

		if (GUILayout.Button ("accountDetails", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) 
		{
			Hashtable table = GSApi.accountDetails ();
			 Debug.Log(table["displayName"]);

			
			oNGUIState = OnGUIState.none;
		}
		if (GUILayout.Button ("challenge sceen", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) 
		{
			GameGlobals.loading = true;
			Application.LoadLevelAsync("FriendsList");
			
			oNGUIState = OnGUIState.none;
		}
		
	}
	
	


	
}

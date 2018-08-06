using UnityEngine;
using System.Collections;
using GameSparksApi;

public class GameSparksTestUI : MonoBehaviour
{
	
	private Queue myLogQueue = new Queue();
	private string myLog = "";
	private string fbToken = "accessToken";
	private string HSScore = "HS";
	private string HSLevel = "GL";
	private string dismissMessageId = "messageId";
	private string buyGoodQty = "Qty";
	private string buyGoodShortCode = "shortCode";
	private string acceptChallengeInstanceId = "Id";
	private string acceptChallengeMessage = "message";
	private int itemHeight = 30;
	private int itemWidth = 200;
	
	private GameSparks GSApi;
	
	
	void Awake ()
	{
		Application.RegisterLogCallbackThreaded (HandleLog);
		
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
	
	void Start(){
		GSApi = GetComponent<GameSparks>();
		
	}

	void HandleGSMessageReceived (object sender, GSMessageReceivedEventArgs e)
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
			
		
	}
	
	void handleMessage(){
		
	}
	
	void HandleLog (string logString, string stackTrace, LogType logType)
	{
		if(myLogQueue.Count > 30){
			myLogQueue.Dequeue();
		}
		myLogQueue.Enqueue(logString);
		myLog = "";
		
		foreach(string logEntry in myLogQueue.ToArray()){
			myLog = logEntry + "\n\n" + myLog;	
		}
		
		
			
	}
	
	void OnGUI ()
	{
		
		// center labels
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.textField.alignment = TextAnchor.MiddleCenter;

		//GUILayout.BeginVertical();
		
		if (GUILayout.Button ("Clear Log", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			myLog = "";
			myLogQueue.Clear();
		}
		
		if (GUILayout.Button ("accountDetails", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.accountDetails ();
		}
		GUILayout.BeginHorizontal ();
		
		if (GUILayout.Button ("facebookConnect", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.facebookConnect (fbToken);
		}
		
		fbToken = GUILayout.TextField (fbToken, GUILayout.Width (itemWidth), GUILayout.Height (itemHeight));
		
		GUILayout.EndHorizontal ();
		
		
		
		GUILayout.BeginHorizontal ();
		
		if (GUILayout.Button ("logEvent - HS", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			int hsScoreInt = 0;
			int hsLevelInt = 0; 
			if (int.TryParse (HSScore, out hsScoreInt) && int.TryParse (HSLevel, out hsLevelInt)) {
				GSApi.logEvent ("HS", new EventVal ("HS", int.Parse (HSScore)), new EventVal ("GL", int.Parse (HSLevel)));
			} else {
				 Debug.Log ("Non numeric values....");	
			}
		}
		
		HSScore = GUILayout.TextField (HSScore, GUILayout.Width (itemWidth/2), GUILayout.Height (itemHeight));
		HSLevel = GUILayout.TextField (HSLevel, GUILayout.Width (itemWidth/2), GUILayout.Height (itemHeight));		
		
		GUILayout.EndHorizontal ();
		
		GUILayout.BeginHorizontal ();
		
		if (GUILayout.Button ("buyVirtualGoodWithCurrency", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			int buyGoodQtyInt = 0;
			if (int.TryParse (buyGoodQty, out buyGoodQtyInt)) {
				GSApi.buyVirtualGoodWithVirtualCurrency (buyGoodShortCode, buyGoodQtyInt, 3);
			} else {
				 Debug.Log ("Non numeric values....");	
			}
		}
		
		buyGoodShortCode = GUILayout.TextField (buyGoodShortCode, GUILayout.Width (itemWidth/2), GUILayout.Height (itemHeight));
		buyGoodQty = GUILayout.TextField (buyGoodQty, GUILayout.Width (itemWidth/2), GUILayout.Height (itemHeight));		
		
		GUILayout.EndHorizontal ();
		
		
		if (GUILayout.Button ("listAchievements", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listAchievements ();
		}
		
		if (GUILayout.Button ("listGameFriends", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listGameFriends ();
		}
		
		if (GUILayout.Button ("listVirtualGoods", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listVirtualGoods ();
		}
		
		if (GUILayout.Button ("listChallengeTypes", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listChallengeTypes ();
		}
		
		if (GUILayout.Button ("listIssuedChallenges", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listIssuedChallenges();
		}
		
		if (GUILayout.Button ("listRunningChallenges", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listRunningChallenges();
		}
		
		if (GUILayout.Button ("listMessages" , GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.listMessages();
		}
		
		GUILayout.BeginHorizontal ();
		
		if (GUILayout.Button ("dismissMessage", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.dismissMessage (dismissMessageId);
		}
		
		dismissMessageId = GUILayout.TextField (dismissMessageId, GUILayout.Width (itemWidth), GUILayout.Height (itemHeight));
		
		GUILayout.EndHorizontal ();
		
		GUILayout.BeginHorizontal ();
		
		if (GUILayout.Button ("acceptChallenge", GUILayout.Width (itemWidth), GUILayout.Height (itemHeight))) {
			GSApi.acceptChallenge (acceptChallengeInstanceId, acceptChallengeMessage);
		}
		
		acceptChallengeInstanceId = GUILayout.TextField (acceptChallengeInstanceId, GUILayout.Width (itemWidth/2), GUILayout.Height (itemHeight));
		acceptChallengeMessage = GUILayout.TextField (acceptChallengeMessage, GUILayout.Width (itemWidth/2), GUILayout.Height (itemHeight));
		
		GUILayout.EndHorizontal ();

		GUI.TextArea (new Rect (420, 5, Screen.width - 425, Screen.height - 10), myLog);
		
		Hashtable logEvent = new Hashtable();
		logEvent.Add("@class", ".LogEventRequest");
		logEvent.Add("HS", 1234);
		logEvent.Add("GL", "TRACK1");
		
		
	}
}

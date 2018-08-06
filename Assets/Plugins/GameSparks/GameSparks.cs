using UnityEngine;
using System.Collections;
using System;
using GameSparksApi;
 



public struct Friend
{
	public string id;
	public string displayName;
	public bool online;
}
/*
public struct Challenge
{
	public string leaderboardName;
	public string name;
	public string challengeShortCode;
	public string description;
	public string tags;
}
*/
public struct Challenge
{
	public string id;
	public string displayName;
	public double pot;
	public enum ChallengeStatus {waiting = 0, running, issues, recieved, complete, declined, unknown, empty};
	public ChallengeStatus status;
	public string ends;
}

public struct Challenger
{
	public string id;
	public string displayName;
	public int topScore;
	public int attempts;
}


public class GameSparks : MonoBehaviour , GSApi {
	
	//Declaring these static & readonly hides them in the IL code
	
	//private readonly string serviceUrl = "ws://localhost:8080/websockets/22dTBsgAElpu";
	//private readonly string serviceUrl = "wss://gsbeta-service2.gamesparks.net/websockets/yFysergDa0Ya4WAM";
	
	//private readonly string serviceUrl = "wss://service.gamesparks.net/websockets/yFysergDa0Ya4WAM";
	//private readonly string appSecret = "3CmcM4U_M5oFacqNkUhELG4JMjgYo6sx";
	
	//private readonly string serviceUrl = "wss://portal.gamesparks.net/service/websockets/22dTBsgAElpu";
	
	//private readonly string serviceUrl = "wss://preview.gamesparks.net/websockets/22dTBsgAElpu";
	private readonly string serviceUrl = "wss://service.gamesparks.net/websockets/22dTBsgAElpu";
	private readonly string appSecret = "grsSZnyydqvCwFSs5iCk2hLP9q44GKqT";
	
	
	private GSConnector connector;
		
	public Hashtable userDetails{ get; set; }
	public Hashtable achievements{get; private set;}
	public Hashtable virtualGoods{get; private set;}
	public Hashtable challengeTypes{get; private set;}
	public String sessionId{ get; set; }
	private ArrayList messages;
	
	
	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad (this);
		messages = new ArrayList();
		connector = new GSConnector (serviceUrl, appSecret, this);
		
	}
	void Start () {

		
	}
	public bool OfflineMode()
	{
		if(connector!= null)
		{
			return connector.offlineMode;
		}
		return true;
	}
	void OnApplicationFocus(bool focusStatus) 
	{
		/*
		 Debug.Log("focus");
		 Debug.Log(focusStatus);
		if(!focusStatus)
		{
			this.Close();
			connector = null;
		}
		else if(connector == null)
		{
			connector = new GSConnector (serviceUrl, appSecret, this);
		}
		*/
	}
	
	void OnApplicationQuit() {
        endSession ();
		connector = null;
    }
	
	
	void OnDestroy () {
		endSession ();
		connector = null;
	}
	
	#if UNITY_IOS
		private String deviceOS = "IOS";
	#elif UNITY_ANDROID
		private String deviceOS = "AND";
	#elif UNITY_EDITOR || UNITY_WEB_PLAYER
		//OK, does not make sense... but there is no WP8 build yet...
		private String deviceOS = "WP8";
	#endif
	
	public void endSession(){
		if(sessionId != null)
		{
			Hashtable data = new Hashtable ();
			data.Add ("@class", ".EndSessionRequest");
			data.Add ("sessionId", sessionId);
			connector.send(data);
			sessionId = null;
			Close ();
		}
	}
	
	public void startTimer(string key){
		startTimer(key, null);	
	}
	
	public void startTimer(string key, Hashtable data){
		if(data == null){
			data = new Hashtable();	
		}
		data.Add ("@class", ".AnalyticsRequest");
		data.Add ("key", key);
		data.Add ("start", true);
		connector.send (data);
	}
	
	public void endTimer(string key){
		endTimer(key, null);	
	}
	
	public void endTimer(string key, Hashtable data){
		if(data == null){
			data = new Hashtable();	
		}
		data.Add ("@class", ".AnalyticsRequest");
		data.Add ("key", key);
		data.Add ("end", true);
		connector.send (data);
	}
	
	public void analyse(string key){
		analyse(key, null);
	}
	public void analyse(string key, Hashtable data){
		if(data == null){
			data = new Hashtable();	
		}
		data.Add ("@class", ".AnalyticsRequest");
		data.Add ("key", key);
		connector.send (data);
	}
	
	public Hashtable accountDetails ()
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".AccountDetailsRequest");
		return connector.send (data);
	}
	
	public Hashtable facebookConnect (String accessToken)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".FacebookConnectRequest");
		data.Add ("accessToken", accessToken);
		return connector.send (data);
	}
	
	public Hashtable logEvent (String eventKey, params EventVal[] values)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".LogEventRequest");
		data.Add ("eventKey", eventKey);
		foreach (EventVal eventVal in values) {
			data.Add (eventVal.getKey (), eventVal.getVal ());	
		}
		return connector.send (data);
	}
	public Hashtable getDownloadURL(string shortCode)
	{
		 Debug.Log("fetch url");
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".GetDownloadableRequest");
		data.Add ("shortCode", shortCode);
		return connector.send (data);
	}
	
	public Hashtable logChallengeEvent (String eventKey, String challengeId, Hashtable values)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".LogChallengeEventRequest");
		data.Add ("eventKey", eventKey);
		data.Add ("challengeId", challengeId);
		foreach (string key in values.Keys) {
			data.Add (key, values [key]);	
		}
		return connector.send (data);
	}
	public Hashtable logHighScore(int score, string shortCode)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".LogEventRequest");
		data.Add ("eventKey", "HS");
		data.Add ("HS", score);
		data.Add ("GL", shortCode);	
		return connector.send (data);
	}
	public Hashtable logChallengeScore(String challengeId, int score, string shortCode)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".LogChallengeEventRequest");
		data.Add ("eventKey", "HS");
		data.Add ("challengeInstanceId", challengeId);
		data.Add ("HS", score);	
		data.Add ("GL", shortCode);	
		
		return connector.send (data);
	}
	public Hashtable registerForPush (String pushId)
	{
		#if UNITY_IOS
		 String deviceOS = "IOS";
		#elif UNITY_ANDROID
		 String deviceOS = "ANDROID";
		#elif UNITY_EDITOR || UNITY_WEB_PLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		
		 String deviceOS = "WP8";
		#endif
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".PushRegistrationRequest");	
		data.Add ("pushId", pushId);	
		data.Add ("deviceOS", deviceOS);
		return connector.send (data);
	}
	
	public Hashtable listVirtualGoods ()
	{
		if(virtualGoods == null){
			Hashtable data = new Hashtable ();
			data.Add ("@class", ".ListVirtualGoodsRequest");	
			virtualGoods = connector.send (data);
		}
		return virtualGoods;
	}
	
	public Hashtable buyVirtualGoodWithVirtualCurrency (String shortCode, int quantity, int currencyType)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".BuyVirtualGoodsRequest");	
		data.Add ("shortCode", shortCode);
		data.Add ("quantity", quantity);
		data.Add("currencyType", currencyType);
		return connector.send (data);
	}
	
	public Hashtable buyVirtualGoodFromStore (String receipt, String signature)
	{
		Hashtable data = new Hashtable ();
#if !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN
		switch (deviceOS) {
		case "IOS":
			data.Add ("@class", ".IOSBuyGoodsRequest");
			data.Add ("receipt", receipt);
			break;
		case "AND":
			data.Add ("@class", ".GooglePlayBuyGoodsRequest");
			data.Add ("signature", signature);
			data.Add ("signedData", receipt);
			break;
		case "WP8":
			data.Add ("@class", ".WindowsBuyGoodsRequest");
			data.Add ("receipt", receipt);
			break;
		}
#else
		data.Add ("@class", ".WindowsBuyGoodsRequest");
		data.Add ("receipt", receipt);
#endif
		return connector.send (data);
	}
	
	public Hashtable listAchievements ()
	{
		if(achievements == null){
			Hashtable data = new Hashtable ();
			data.Add ("@class", ".ListAchievementsRequest");	
			achievements = connector.send (data);
		}
		return achievements;
	}
	
	public Hashtable listGameFriends ()
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListGameFriendsRequest");	
		return connector.send (data);
	}
	
	public Hashtable listChallengeTypes ()
	{
		if(challengeTypes == null){
			Hashtable data = new Hashtable ();
			data.Add ("@class", ".ListChallengeTypeRequest");	
			challengeTypes = connector.send (data);
		}
		return challengeTypes;
	}
	
	
	public Hashtable listChallengeDetails (string challengeID)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".GetChallengeRequest");	
		data.Add ("challengeInstanceId", challengeID);	
		
		return connector.send (data);
	}
	
	public Hashtable createChallenge(string challengeShortCode, string challengeMessage, int virtualCurrencyWager, DateTime endTime, int maxTurns, params string[] usersToChallenge){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".CreateChallengeRequest");	
		data.Add ("challengeShortCode", challengeShortCode);	
		data.Add ("challengeMessage", challengeMessage);	
		data.Add ("currency3Wager", virtualCurrencyWager);	
		data.Add ("endTime", endTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mmZ"));	
		data.Add ("maxAttempts", maxTurns);	
		data.Add ("usersToChallenge", usersToChallenge);	
		return connector.send (data);
	}
	
	public Hashtable listIssuedChallenges(){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListChallengeRequest");	
		data.Add ("state", "ISSUED");	
		return connector.send (data);
	}
	
	public Hashtable listRunningChallenges(){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListChallengeRequest");	
		data.Add ("state", "RUNNING");	
		return connector.send (data);
	}
	public Hashtable ListChallengeScores(string level, string id, int entries){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".LeaderboardDataRequest");	
		data.Add ("leaderboardShortCode", level);	
		data.Add ("challengeInstanceId", id);	
		data.Add ("entryCount",entries);
		return connector.send (data);
	}
	public Hashtable Leaderboards(string shortCode)
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".SocialLeaderboardDataRequest");	
		data.Add ("leaderboardShortCode", shortCode);
		data.Add ("entryCount",10);
		return connector.send (data);
		
		
	}
	public Hashtable listCompletedChallenges(){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListChallengeRequest");	
		data.Add ("state", "COMPLETE");	
		return connector.send (data);
	}
	public Hashtable listRecievedChallenges(){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListChallengeRequest");	
		data.Add ("state", "RECEIVED");	
		return connector.send (data);
	}
	
	public Hashtable acceptChallenge(String challengeInstanceId, String message){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".AcceptChallengeRequest");	
		data.Add ("challengeInstanceId", challengeInstanceId);
		if(message!= null)
			data.Add ("message", message);
		return connector.send (data);
	}
	
	public Hashtable declineChallenge(String challengeInstanceId, String message){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".DeclineChallengeRequest");	
		data.Add ("challengeInstanceId", challengeInstanceId);
		data.Add ("message", message);
		return connector.send (data);
	}
	
	public Hashtable chatOnChallenge(String challengeInstanceId, String message){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ChatOnChallengeRequest");	
		data.Add ("challengeInstanceId", challengeInstanceId);
		data.Add ("message", message);
		return connector.send (data);
	}
	public Hashtable GetMessages()
	{
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListMessageRequest");	
		return connector.send(data);
	}
	public ArrayList listMessages(){
		if(messages == null){
			refreshMessages();
		}
		if(messages == null){
			messages = new ArrayList();
		}
		
		return messages;
	}
	
	private void refreshMessages(){
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".ListMessageRequest");	
		messages = (ArrayList)connector.send(data)["messages"];
	}
	
	public Hashtable dismissMessage(String messageId){
		
		Hashtable data = new Hashtable ();
		data.Add ("@class", ".DismissMessageRequest");	
		data.Add ("messageId", messageId);	
		Hashtable ret = connector.send (data);
		refreshMessages();
		return ret;
	}
	
	public Hashtable sendData(Hashtable data){
		return connector.send(data);	
	}
		
	
	private EventHandler<GSMessageReceivedEventArgs> m_MessageReceived;

	public event EventHandler<GSMessageReceivedEventArgs> GSMessageReceived {
		add { m_MessageReceived += value; }
		remove { m_MessageReceived -= value; }
	}
	
	public void FireMessageReceived (Hashtable message)
	{
		messages.Add(message);
		if (m_MessageReceived == null)
			return;

		m_MessageReceived (this, new GSMessageReceivedEventArgs (message));
		
	}
	public void Close()
	{
  		if(connector != null)
  		{
			connector.Close();
  		}
 	}
	
}


 
   

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
/*
 *This script manages the many external api initialization routines.
*/

#if UNITY_ANDROID
using FacebookAccess = FacebookAndroid;
#elif UNITY_IPHONE
using FacebookAccess = FacebookBinding;
#endif

public class Init : MonoBehaviour {
	// Project Number on Google API Console
	private static readonly string[] SENDER_IDS = {"1002606205556"};
	public DLCManager dlcManager;
	private string _text = "(null)";
	public GameSparks GSApi;
	private Queue myLogQueue = new Queue();
	private string myLog = "";
	private string fbToken = "accessToken";
	private static readonly string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3yFcD9mhSTyZmNUiWvJu6NE3O3PmUgq82njLzZCTfdAf/0du2q4+KnVkcMMFg4T9RwZ07gd3cX34qgO+05ke/czFtcqZHqwNXp5qiEX3+XeYKY1ySJm9xGfJXOTJflvhsJBLwVfJnpmCxSB3+epm8QYP1TnFV2zfDNauVea/C7VBaw5l237Ia+H2qoFGIEZdJYLTPMFlKMnXGDlBsfnqzpVavaLosMkPARdW0uAWQNnHukNwD8hOcG83GhcWkkwGhrwuo9pskgKCBtLoMyd9eB6c96sQgfkuQiUj858ciMOJwwLD9QhEFWAxdwcGBM2fEkLYspFo1dW3f4olFEJGTQIDAQAB";
	string[] skus = {"bbcp1","bbcp2","bbcp3"};	
	
	WWW www; 
	void Start () {
		GameGlobals.messageCount = PlayerPrefs.GetInt("challengeMessages", 0);
		GameGlobals.Songs =new Hashtable();
		GameGlobals.otherIDs = new ArrayList();
		//string trackFolder = "Assets/Audio/Resources/Tracks/";
		if (Debug.isDebugBuild) Debug.Log("loading track names");
		string trackFolder = "Tracks/";
		GameGlobals.Songs.Add("Clu - Ruby",new AudioTrack(trackFolder+"01/Songs/Clu - Ruby",trackFolder+"01/AlbumArt/Clu", true,false,"TKHSCLURUB"));
		GameGlobals.Songs.Add("Clu + Ft. Lindstrøm - Rà-àkõ-st",new AudioTrack(trackFolder+"01/Songs/Clu + Ft. Lindstrøm - Rà-àkõ-st",trackFolder+"01/AlbumArt/Clu", true,false, "TKHSCLURAA"));
		GameGlobals.Songs.Add("Cosmic Boy - Survival",new AudioTrack(trackFolder+"02/Songs/Cosmic Boy - Survival",trackFolder+"02/AlbumArt/Cosmic Boy", true,false,"TKHSCOSSUR"));
		GameGlobals.Songs.Add("Knife City - Bad News",new AudioTrack(trackFolder+"03/Songs/Knife City - Bad News",trackFolder+"03/AlbumArt/Knife City", true,false,"TKHSKNIBAD"));
		GameGlobals.Songs.Add("Knife City - Just Trash",new AudioTrack(trackFolder+"03/Songs/Knife City - Just Trash",trackFolder+"03/AlbumArt/Knife City", true,false,"TKHSKNIJUS"));
		GameGlobals.Songs.Add("Polygon APE - IncrediBULL",new AudioTrack(trackFolder+"04/Songs/Polygon APE - IncrediBULL",trackFolder+"04/AlbumArt/Polygon APE", true,false,"TKHSPOLINC"));
		GameGlobals.Songs.Add("Polygon APE - Riff Raff",new AudioTrack(trackFolder+"04/Songs/Polygon APE - Riff Raff",trackFolder+"04/AlbumArt/Polygon APE", true,false,"TKHSPOLRIF"));
		GameGlobals.Songs.Add("RIOT AKKT - TIGER CHILD",new AudioTrack(trackFolder+"05/Songs/RIOT AKKT - TIGER CHILD",trackFolder+"05/AlbumArt/Riot Akkt", true,false,"TKHSRIOTIG"));
		GameGlobals.Songs.Add("Sabrepulse + Knife City - First Crush",new AudioTrack(trackFolder+"06/Songs/Sabrepulse + Knife City - First Crush",trackFolder+"06/AlbumArt/Sabrepulse", true,false,"TKHSSABFIR"));
		GameGlobals.Songs.Add("Sabrepulse - A Girl I Know",new AudioTrack(trackFolder+"06/Songs/Sabrepulse - A Girl I Know",trackFolder+"06/AlbumArt/Sabrepulse", true,false,"TKHSSABAGI"));
		
		if (Debug.isDebugBuild) Debug.Log(" finished loading track names");
		
		GameGlobals.TileSets = new Hashtable();
			
		string tilesFolder = "Tiles/01/";
		GameGlobals.TileSets.Add("LED", new TileSet("LED", tilesFolder,true,"AAAAAAAAAA"));
		tilesFolder = "Tiles/02/";
		GameGlobals.TileSets.Add("Fruit", new TileSet("Fruit", tilesFolder,true,"BBBBBBBBBB"));
		tilesFolder = "Tiles/03/";
		GameGlobals.TileSets.Add("Stripes", new TileSet("Stripes", tilesFolder,true,"CCCCCCCCCC"));
		
		
		
		/*
		string[] folderNames = Directory.GetFiles("Assets/Audio/Resources/Tracks");
		
		foreach(string folder in folderNames){
			//if(folder != "Assets/Audio/Resources/Tracks/.DS_Store")//macs add this file to this directory
			{
				if(!folder.Contains(".meta") && !folder.Contains(".DS_Store")){
					string[] albumArt = Directory.GetFiles(folder+"/AlbumArt/");
					string[] songNames = Directory.GetFiles(folder+"/Songs/");
					
					foreach(string currentSong in songNames){
						if(!currentSong.Contains(".meta") && !folder.Contains(".DS_Store")){
							if( !albumArt[0].Contains(".DS_Store") )
								GameGlobals.Songs.Add( new AudioTrack( folder+"/Songs/"+currentSong, folder+"/AlbumArt/"+albumArt[0] ) );
							else
								GameGlobals.Songs.Add( new AudioTrack( folder+"/Songs/"+currentSong, folder+"/AlbumArt/"+albumArt[1] ) );
						}
					}
				}
			}
		}
		
		
		foreach(AudioTrack current in GameGlobals.Songs){
			if (Debug.isDebugBuild) Debug.LogError(current.song + " - " + current.albumArt);
		}
		*/
		if (Debug.isDebugBuild) Debug.Log("setting selected track");
		GameGlobals.selectedTrack = ( (AudioTrack) GameGlobals.Songs["Clu - Ruby"] );
		
		if (Debug.isDebugBuild) Debug.Log("finished setting selected track");
		
		
		GameGlobals.sound = (PlayerPrefs.GetInt("Sound") == 0) ? true: false;
		GameGlobals.messages = (PlayerPrefs.GetInt("Messages") == 0) ? true : false;
		
		//everybody starts with 3 multipliers
		if(PlayerPrefs.GetInt("FirstTimePlaying") == 0){
			PlayerPrefs.SetInt("FirstTimePlaying", 1);
			//PlayerPrefs.SetInt("InGameCurrency", 0);
			PlayerPrefs.SetInt("X2s", 3);
		}
		
		if(PlayerPrefs.GetString("TileSet") == "" || PlayerPrefs.GetString("TileSet") == null){
			PlayerPrefs.SetString("TileSet", "LED");
		}
		
		GameGlobals.selectedTiles = ( (TileSet) GameGlobals.TileSets[PlayerPrefs.GetString("TileSet")] );
		
		
		
		DontDestroyOnLoad (this);
		SetCallbacks();
#if !UNITY_EDITOR && !UNITY_WEB_PLAYER && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN		
		
		FacebookAccess.init(false);
		
#else
		GSApi.facebookConnect("CAAHZBQNnoPZBMBAFBJJXuqFhEbWkjfEKOwUNzZAGT2nZCvQkLZBB43fbk2M5F3YoDc1nRSaEDCA6zspcZAL9hFhRQjNSYEkLVtcy8Ha1ewceuXv1CAi41XoudeMCBrmgX8V0yZCGVqqmIhqkXfSqJeSbJSWZBieMuze7N0lEZBK2VDeuggBR4BD2qluDUbGdVOmeCpnar5i6K9sMUPY6GSwZBH");
		//GSApi.facebookConnect("CAAHZBQNnoPZBMBABWZBc7ZA9AnojVXSwDpJ3I5wtUTntrVnuU67EYKxWg9AKVF3UidCGb66K8sdDPZCiizffbaDPfeMlezKBKfRy1mEwhWYm6IKQmSxf89ZAO4EZC6vbmJTZCZA0cCpiNVg1LLbhbhVTZC5W3sZAGYteZAE4QmGzXyCZAMQhYeRlZBlgWLvQHaXtWDd6fRXMVJc3m9I7qAVwkjaS8h");
#endif
		
		if (Debug.isDebugBuild) Debug.Log("setting account info");
		
		Hashtable details = GSApi.accountDetails();

		
		if(!GSApi.OfflineMode() && details["userId"] != null)
		{
			
			if(details["userId"] != null)
			{
				GameGlobals.userID = (string)details["userId"];
			}
			 
			
			if (Debug.isDebugBuild) Debug.Log("finished setting account info");
			GameGlobals.online= true;
			dlcManager.initDLC();//start managing dlc
		}
#if UNITY_ANDROID && !UNITY_EDITOR
		
		if (Debug.isDebugBuild) Debug.Log("setting GCM");
		GCM.Initialize ();
		IAP.init( key );
		
		if(!GCM.IsRegistered()){
			GCM.Register();
		}
		if(!GCM.IsRegisteredOnServer ())
		{
			GCM.SetRegisteredOnServer(true);
		}
		if (Debug.isDebugBuild) Debug.Log("finished setting GCM");
#endif
			
		
	}
	
	void completionHandler( string error, object result )
	{
		if( error != null )
			if (Debug.isDebugBuild) Debug.LogError( error );
		else
			Prime31.Utils.logObject( result );
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
	// Update is called once per frame
	void Update () {
	
	}
	void OnApplicationFocus(bool focusStatus) 
	{
		PlayerPrefs.SetInt("challengeMessages",GameGlobals.messageCount);
	}
	void RetriveDLCCatalogue()
	{
		
		Hashtable table = GSApi.getDownloadURL("DLC");
		www = new WWW((string)table["url"]);
		StartCoroutine("DownloadXML");
	}
	IEnumerator DownloadXML()
	{
		yield return www;
		if(www.error == null)
		{
			if (Debug.isDebugBuild) Debug.Log("download Complete");
			CheckXML(www.bytes);
			
		}
	}
	void CheckXML(byte[] bytes)
	{
		//
		
		FileStream fs=new FileStream("Assets/Resources/dlc.xml", FileMode.Create);
		BinaryWriter w = new BinaryWriter(fs);
		w.Write(bytes);
		w.Close();
		fs.Close();
		if (Debug.isDebugBuild) Debug.Log("save Complete");
		
		
		
		TextAsset textAsset = (TextAsset) Resources.Load("dlc", typeof(TextAsset)); 
		
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml (textAsset.text );
		
		if(!System.IO.File.Exists("Assets/Resources/"+((xmldoc.GetElementsByTagName("currentBundle")[0]).InnerText)+ ".unity3d"))
		{
			LoadNewImagePack((xmldoc.GetElementsByTagName("currentBundle")[0]).InnerText);
			
		}
		
	}
	void LoadNewImagePack(string currentBundle)
	{
		if (Debug.isDebugBuild) Debug.Log(currentBundle);
		
		Hashtable table = GSApi.getDownloadURL(currentBundle);
		www = new WWW((string)table["url"]);
		StartCoroutine(DownloadBundle(currentBundle));
	}

	IEnumerator DownloadBundle(string currentBundle)
	{
		yield return www;
		if(www.error == null)
		{
			if (Debug.isDebugBuild) Debug.Log("download Complete");
			
			AssetBundle bundle = www.assetBundle;
			
			if (Debug.isDebugBuild) Debug.Log("saving");
			FileStream fs=new FileStream("Assets/Resources/"+currentBundle+".unity3d", FileMode.Create);
			BinaryWriter w = new BinaryWriter(fs);
			w.Write(www.bytes);
			w.Close();
			fs.Close();
		}
	}
	void SetCallbacks()
	{
		Application.RegisterLogCallbackThreaded (HandleLog);
		/*GSApi.GSMessageReceived += (GS, args)=>
		{
			if(args.Message["@class"] as string == ".ChallengeAcceptedMessage"){
#if UNITY_ANDROID && !UNITY_EDITOR 
				GCM.ShowToast("Challenge Accepted");
#else
				if (Debug.isDebugBuild) Debug.Log("Challenge Accepted");
#endif
			}
			else if(args.Message["@class"] as string == ".ChallengeDeclinedMessage"){
#if UNITY_ANDROID && !UNITY_EDITOR 
				GCM.ShowToast("Challenge Declined");
#else
				if (Debug.isDebugBuild) Debug.Log("Challenge Accepted");
#endif
			}
			else if(args.Message["@class"] as string == ".ChallengeChatMessage"){
#if UNITY_ANDROID && !UNITY_EDITOR 
				GCM.ShowToast("Challenge Chat Recieved");
#else
				if (Debug.isDebugBuild) Debug.Log("Challenge Chat Recieved");
				GameGlobals.messageCount++;
#endif
			}
			
		};*/
#if UNITY_ANDROID && !UNITY_EDITOR 			
		FacebookManager.sessionOpenedEvent+=delegate() 
		{
			if (Debug.isDebugBuild) Debug.Log("facebook callback worked");
			
			
			
			//we assume that this can will only be called if session is invalid or we are on a new device. 
			
			Hashtable response = GSApi.facebookConnect(FacebookAccess.getAccessToken());

			if((string)response["@class"] ==".AuthenticationResponse" && (string)response["authToken"] != null){
				GameGlobals.online= true;
				Hashtable details = GSApi.accountDetails();
				

				if(details["userId"] != null)
				{
					GameGlobals.userID = (string)details["userId"];
				}
				if(!GCM.IsRegistered())
					GCM.Register();
				GSApi.registerForPush(GCM.GetRegistrationId());
			}
			else{
				GameGlobals.online= false;
			}
				
		};
#endif		
		
		

		
#if UNITY_ANDROID && !UNITY_EDITOR	

	
		
		GCM.SetErrorCallback ((string errorId) => {
			if (Debug.isDebugBuild) Debug.Log ("Error!!! " + errorId);
			GCM.ShowToast ("Error!!!");
			_text = "Error: " + errorId;
		});

		GCM.SetMessageCallback ((Dictionary<string, object> table) => {
			if (Debug.isDebugBuild) Debug.Log ("Message!!!");
			GCM.ShowToast ("Message!!!");
			_text = "Message: " + System.Environment.NewLine;
			foreach (var key in  table.Keys) {
				_text += key + "=" + table[key] + System.Environment.NewLine;
			}
		});

		
		
		
		GCM.SetRegisteredCallback ((string registrationId) => {
			//if(FacebookAccess.isSessionValid() && 
		});

#endif
	}
	void OnEnable()
	{
#if UNITY_WEB_PLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		
#else
		if (Debug.isDebugBuild) Debug.Log("enabling google IAB");

		
		
		GoogleIABManager.consumePurchaseSucceededEvent += 		( purchase )=>
		{
			GSApi.buyVirtualGoodFromStore(purchase.originalJson,purchase.signature);
			if (Debug.isDebugBuild) Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
			//GSApi.buyVirtualGoodFromStore(purchase.originalJson,purchase.signature);
		};
		GoogleIABManager.consumePurchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.queryInventorySucceededEvent += ( purchases, skus ) =>
		{
			string[] products = new string[purchases.Count];
			//var convertedProducts = new List<IAPProduct>();
			for(int i =0; i < purchases.Count; i ++)
			{
				products[i] = purchases[i].productId;
				if (Debug.isDebugBuild) Debug.Log( purchases[i].productId);
				if (Debug.isDebugBuild) Debug.Log( purchases[i].packageName);
			}
			GoogleIAB.consumeProducts(products);
			
		};
		
		GoogleIABManager.billingSupportedEvent +=billingSupportedEvent;
#endif

		
	}


	void OnDisable()
	{
		
		
		
		
		
	}


	
#if UNITY_WEB_PLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
	
#else
	void billingSupportedEvent()
	{
		if (Debug.isDebugBuild) Debug.Log("billing supported");
		GoogleIAB.queryInventory(skus);
		
	}


	void billingNotSupportedEvent( string error )
	{
		if (Debug.isDebugBuild) Debug.Log( "billingNotSupportedEvent: " + error );
	}

	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		if (Debug.isDebugBuild) Debug.Log( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) );
		Prime31.Utils.logObject( purchases );
		Prime31.Utils.logObject( skus );
	}
#endif

	void queryInventoryFailedEvent( string error )
	{
		if (Debug.isDebugBuild) Debug.Log( "queryInventoryFailedEvent: " + error );
	}


	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
	
		if (Debug.isDebugBuild) Debug.Log( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
	}
	

#if UNITY_WEB_PLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
	
#else
	void purchaseSucceededEvent( GooglePurchase purchase )
	{
		
		if (Debug.isDebugBuild) Debug.Log( "purchaseSucceededEvent: " + purchase );
		
	}
#endif


	void purchaseFailedEvent( string error )
	{
		if (Debug.isDebugBuild) Debug.Log( "purchaseFailedEvent: " + error );
	}





	void consumePurchaseFailedEvent( string error )
	{
		if (Debug.isDebugBuild) Debug.Log( "consumePurchaseFailedEvent: " + error );
	}
}

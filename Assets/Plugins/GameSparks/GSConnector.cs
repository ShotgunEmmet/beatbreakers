using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using WebSocketSharp;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

	public class GSConnector
	{
		public bool offlineMode = false;
		private WebSocket webSocketClient;
		
		private String appSecret;
		
		private String webSocketUrl;
		
		private String serviceUrl;
		
		public String authToken{get;set;}
		
		private Dictionary<String, GSAutoResetEvent> pendingRequests = new Dictionary<String, GSAutoResetEvent>();

		private AutoResetEvent m_OpenedEvent = new AutoResetEvent(false);
		private AutoResetEvent m_InitialisedEvent = new AutoResetEvent(false);
		
		private GSApi gsApi;
		
		private int failedConnectAttempts = 0;
		
		public bool initialised{get;private set;}
		private Boolean initialising=false;
		
		private string os = SystemInfo.operatingSystem;
		private string platform = Application.platform.ToString();
		private string deviceId = SystemInfo.deviceUniqueIdentifier;
		
		public GSConnector (String serviceUrl, String appSecret, GSApi gsApi)
		{
			this.appSecret = appSecret;
			this.gsApi = gsApi;
			initialised = false;
		
			ServicePointManager.ServerCertificateValidationCallback = Validator;
			
			webSocketUrl= serviceUrl;
			this.serviceUrl = webSocketUrl;
			
			 Debug.Log(String.Format ("webSocketUrl: {0}", webSocketUrl));
			authToken = PlayerPrefs.GetString(serviceUrl +".authToken");
			
			if(authToken == null || authToken.Length == 0){
				authToken = "0";	
			}
			initialising = true;	
			connect ();	
			m_InitialisedEvent.WaitOne(1000);
			connect ();	
			m_InitialisedEvent.WaitOne(1000);
			
		}
		
		public bool authenticated(){
			if(authToken == null || authToken.Equals("0") || authToken.Length==0){
				return false;
			}
			return true;
		}
	
		private Boolean WsReady(){
			if(webSocketClient == null){
				return false;
			}
			if(webSocketClient.ReadyState == WsState.OPEN || webSocketClient.ReadyState == WsState.CONNECTING){
				return true;
			}
			
			return false;
		}

		private void connect ()
		{
			offlineMode = false;
			if(failedConnectAttempts > 3){
				 Debug.LogError("Reached max failures, exiting");
				offlineMode = true;
				
				failedConnectAttempts = 0;
				return;
				
			}
			
			lock(gsApi){
				
				if (!WsReady()) {
					if(webSocketClient != null)
					{
						webSocketClient.Connect();
					}
					else 
					{
						webSocketClient = new WebSocket(webSocketUrl, 
								HandleWebSocketClientOnOpen, 
								HandleWebSocketClientOnMessage, 
								HandleWebSocketClientOnError, 
								HandleWebSocketClientOnClose, null, new String[0]);
					}
					
					if (!m_OpenedEvent.WaitOne(5000)){
						failedConnectAttempts++;
					}
					
				}
			}
		}

		
		private void HandleWebSocketClientOnError (object sender, ErrorEventArgs e)
		{
			//I we get an error it could be an endpoint issue, reset the endpoint url
			 Debug.Log("ERROR " + e.ToJSON());
			//webSocketClient = null;
			failedConnectAttempts ++;
			
		}

		private void HandleWebSocketClientOnOpen (object sender, EventArgs e)
		{
			m_OpenedEvent.Set();
			 Debug.Log("OPENED " + e.ToJSON());
		}

		private void HandleWebSocketClientOnClose (object sender, CloseEventArgs e)
		{
			 Debug.Log("CLOSED " + e.ToJSON());
			
			if(initialised)
			{
				connect();
			} 
			else 
			{
				webSocketClient = null;
			}
			
			if(initialising){
				m_InitialisedEvent.Set();
			}
		}
		
		private void HandleWebSocketClientOnMessage (object sender, MessageEventArgs e)
		{
			String message = e.Data.Trim();
			if(message.Length > 0){
				 Debug.Log("RECV:" + message);
				
				
				Hashtable response = (Hashtable)JSON.Deserialize (e.Data);
				
				String messageClass = response ["@class"].ToString();
				
				if (messageClass.EndsWith ("Response")) {
					if(response["error"] == null){
						processSuccessResponse(response, messageClass);
					} else {
						processErrorResponse(response, messageClass);	
					}
						
				} else {
					//It's a push message, do something else...
					 Debug.Log("RECV:MESSAGE start");
					gsApi.FireMessageReceived(response);
					 Debug.Log("RECV:MESSAGE done");
				}
			} else {
				 Debug.Log("RECV:HEARTBEAT");
			}
		}
		
		
		public void Close(){
			initialised = false;
			if(WsReady()){
				webSocketClient.Close ();
			}
		}
		
		public void sendNoWait(Hashtable data){
			//Connect if required
			if (!WsReady()) {
				connect ();
			}
			
			if (WsReady()) {
			
				String jsonData = JSON.Serialize(data);
			
				 Debug.Log("SEND:" + jsonData);
			
				webSocketClient.Send(jsonData);
			}
		}
		
		public Hashtable send(Hashtable data){
			
			String origAuthToken = authToken;
			Hashtable ret = sendInternal(data);
			if(!authToken.Equals(origAuthToken))
			{
				PlayerPrefs.SetString(serviceUrl +".authToken", authToken);
			}
			return ret;
		}
		
		private Hashtable sendInternal (Hashtable data)
		{	
			String requestId = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
			data["requestId"] = requestId;
			
			GSAutoResetEvent messageReceiveEvent = new GSAutoResetEvent(false);
			pendingRequests.Add(requestId, messageReceiveEvent);
		
			sendNoWait(data);
			
			if (!messageReceiveEvent.WaitOne(5000))
			{
				Hashtable errors = new Hashtable();
				errors.Add("timeout", "No response within 5 secs");
				 Debug.Log("No response within 5 secs");
				pendingRequests.Remove(requestId);
				Hashtable error = new Hashtable();
				error.Add("error", errors);
				return error;
			}
			
			pendingRequests.Remove(requestId);
			return messageReceiveEvent.response;
			
		}
		
		public static bool Validator (object sender, X509Certificate certificate, X509Chain chain, 
                                      SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
		
		private void processSuccessResponse(Hashtable response, String messageClass){
			
			if (response.ContainsKey ("authToken")) 
			{
				//Save this, we'll need it if we have to reconnect
				authToken = response ["authToken"].ToString ();
			}
			
			if (response.ContainsKey ("connectUrl")) 
			{
				webSocketUrl = response ["connectUrl"].ToString ();
				
			}			
			switch(messageClass)
			{
			case ".AuthenticatedConnectResponse":
			
				if(response.ContainsKey("error")){
					return;
				}
				if (!response.ContainsKey ("connectUrl")) 
				{
					if(response.ContainsKey ("nonce"))
					{
						Hashtable nonceResponse = new Hashtable();
						nonceResponse.Add("@class", ".AuthenticatedConnectRequest");
						nonceResponse.Add("hmac", MakeHmac( response["nonce"].ToString()));
						nonceResponse.Add("os", os);
						nonceResponse.Add("platform", platform);
						nonceResponse.Add("deviceId", deviceId);
		
						if(!authToken.Equals("0")){
							nonceResponse.Add ("authToken", authToken);
						}
						
						if(gsApi.sessionId != null){
							nonceResponse.Add("sessionId", gsApi.sessionId);
						}
						sendNoWait (nonceResponse);
					} else {
						if(response.ContainsKey ("sessionId")){
							gsApi.sessionId = response["sessionId"].ToString ();	
							initialising=false;					
							initialised = true;
							m_InitialisedEvent.Set();
						}
					}
				}
				return;
			}
			completeResponse(response);
			
		}
		
		private void processErrorResponse(Hashtable response, String messageClass){
		
			Hashtable errors = (Hashtable)response["error"];
			if(errors["authentication"] != null && errors["authentication"].Equals("NOTAUTHORIZED")){
				authToken = "0";	
			}
			
			completeResponse(response);
		}
		
		private void completeResponse(Hashtable response){
			
			String requestId = response ["requestId"].ToString ();
			if(requestId != null){
				if(pendingRequests.ContainsKey(requestId)){
					GSAutoResetEvent pending = pendingRequests[requestId];
					if(pending != null){
						pending.populateAndSet(response);
					}
				}
			}
		}
		
	
		private string MakeHmac(String strToHmac)
		{
			
			var encoding = new System.Text.UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(appSecret);
			byte[] messageBytes = encoding.GetBytes(strToHmac);
			using (var hmacsha256 = new HMACSHA256(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				string sig = Convert.ToBase64String(hashmessage);
				 Debug.Log("AuthSignature " + strToHmac + " BECAME " + sig);
				return sig;
			}
		}
		
		
		
		~GSConnector ()
		{
			if (WsReady()) {
				initialised = false;
				webSocketClient.Close ();
			}
		}

	}
	
	class GSAutoResetEvent  {
	
		private AutoResetEvent autoResetEvent;
		public Hashtable response {get;set;}
		
		public GSAutoResetEvent(bool initialState){
			autoResetEvent = new AutoResetEvent(initialState);	
		}
		
		public void populateAndSet(Hashtable response){
			this.response = response;
			autoResetEvent.Set();
		}
		
		public bool WaitOne(int millisecondsTimeout){
			return autoResetEvent.WaitOne(millisecondsTimeout);
		}
		
	}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class DLCManager : MonoBehaviour {
	GameSparks GSApi; 
	WWW www; 
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		
	}

	
	public AssetBundle DLCBundle;
	public AudioClip LoadTrack(string shortCode)
	{
		www = new WWW("file://"+ Application.persistentDataPath+"/"+shortCode.Replace("tkhs", "tk")+".unity3d");
		
		AssetBundle bundle = www.assetBundle;
		return bundle.mainAsset as AudioClip;
	}
	public void initDLC()
	{//call after we sign in
		StartCoroutine("CheckDownloads");
	}
	IEnumerator CheckForOwnedDLC()
	{
		
		string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath+"/", "*.unity3d");
		foreach(string s in files)
		{
			if(s != Application.persistentDataPath+"/DLC1.unity3d")
			{
				if (Debug.isDebugBuild) Debug.Log("file://"+ Application.persistentDataPath+"/"+s);
				//www = new WWW("file://"+ Application.dataPath.Replace("Assets","")+s);
				
				//yield return www;
				//www.assetBundle.
				AssetBundle bundle = AssetBundle.CreateFromFile(s.Replace(".unity3s", ""));
				if (Debug.isDebugBuild) Debug.Log(bundle.mainAsset.name);
				//if (Debug.isDebugBuild) Debug.Log(bundle.mainAsset.name);
			}
			else if(s == Application.persistentDataPath+"/DLC1.unity3d")
			{
				if (Debug.isDebugBuild) Debug.Log("file://"+ Application.persistentDataPath+"/"+s);
				www = new WWW("file://"+ Application.persistentDataPath+"/"+s);
				
				yield return www;
				AssetBundle bundle = www.assetBundle;
				TextAsset test =  (TextAsset)bundle.Load("dlc");
				if (Debug.isDebugBuild) Debug.Log(test.text);
			}
		}
	}
	IEnumerator CheckDownloads()
	{
		yield return null;
		Hashtable table;
		if(PlayerPrefs.GetInt("DLCComplete", 1)==0)
		{
			//dlc did not finish
			table = GSApi.getDownloadURL(PlayerPrefs.GetString("DownloadShortCode"));//get url of dlc
			www = new WWW((string)table["url"]);
			PlayerPrefs.SetInt("DLCComplete", 0);
			yield return www;
			if(www.error == null)
			{
				if (Debug.isDebugBuild) Debug.Log("download unfinished Complete");
				if (Debug.isDebugBuild) Debug.Log("saving");
				FileStream fs=new FileStream(Application.persistentDataPath+"/"+PlayerPrefs.GetString("DownloadShortCode")+".unity3d", FileMode.Create);
				BinaryWriter w = new BinaryWriter(fs);
				w.Write(www.bytes);
				w.Close();
				fs.Close();
				PlayerPrefs.SetInt("DLCComplete", 1);
			}
		}
		//download new catalogue
		table = GSApi.getDownloadURL("DLC1");
		www = new WWW((string)table["url"]);
		yield return www;
		if(www.error == null)
		{
			if (Debug.isDebugBuild) Debug.Log("download Complete");
			DLCBundle = www.assetBundle;
			if (Debug.isDebugBuild) Debug.Log("saving");
			FileStream fs=new FileStream(Application.persistentDataPath+"/DLC1.unity3d", FileMode.Create);
			BinaryWriter w = new BinaryWriter(fs);
			w.Write(www.bytes);
			w.Close();
			fs.Close();
			yield return StartCoroutine("CheckXML",DLCBundle);
		}
		
		
		

	}
	
	

	// Update is called once per frame
	void Update () {
	
	}


	IEnumerator CheckXML(AssetBundle bundle)
	{
		Hashtable details = GSApi.accountDetails();
		Hashtable virtualGoods;
		Hashtable costs = GSApi.listVirtualGoods();
		ArrayList goodList = (ArrayList)costs["virtualGoods"];
		
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml (((TextAsset)bundle.Load("dlc", typeof(TextAsset))).text );
		var list =xmldoc.GetElementsByTagName("dlctrack");
		
		virtualGoods = (Hashtable)details["virtualGoods"];
		foreach(XmlElement node in list)
		{
			if(details["virtualGoods"]!= null 
				&& (virtualGoods[node["id"].InnerText.Replace("TKHS", "TK")]!= null))
			{
				if(!System.IO.File.Exists(Application.persistentDataPath+"/"+node["id"].InnerText+".unity3d")){
					if (Debug.isDebugBuild) Debug.Log("download missing track");
					yield return StartCoroutine("DownloadBundle", node["id"].InnerText.Replace("TKHS", "TK"));
				}
				AddDLCTrack(node["artist"].InnerText+"-"+node["track"].InnerText, node["image"].InnerText, node["id"].InnerText, true,0,0);
			}
			else
			{
				int bb = 0; 
				int gs = 0;
				foreach(Hashtable table in goodList)
				{
					if((string)table["shortcode"] == node["id"].InnerText.Replace("TKHS", "TK"))
					{
						gs = (int)(double)table["currency2Cost"];
						bb = (int)(double)table["currency3Cost"];
					}
				}
				AddDLCTrack(node["artist"].InnerText+"-"+node["track"].InnerText, node["image"].InnerText, node["id"].InnerText, false, bb, gs);
				if (Debug.isDebugBuild) Debug.Log("I don't own this track");
			}
				/*node[artist].InnerText
				node[track].InnerText
				node[image].InnerText
				node[id].InnerText*/
					
			if (Debug.isDebugBuild) Debug.LogWarning(node.InnerText);
		}
		
		
	}
	void AddDLCTrack(string title, string albumArt, string shortCode, bool purchased , int bbCost, int gsCost)
	{
		if (Debug.isDebugBuild) Debug.Log("add dlc track to list");
		AudioTrack track = new AudioTrack(title,albumArt, purchased,true,shortCode,bbCost, gsCost);
		
		GameGlobals.Songs.Add(title,track);
	}
	string dlcName = "";
	public bool PurchaseTrack(string name, string shortCode)
	{
		Hashtable response = GSApi.buyVirtualGoodWithVirtualCurrency(shortCode, 1, 2);
		/*if(response["error"] != null)
		{
			return false;
		}*/
		dlcName = name;
		if(response["boughtItems"]!= null)
		{
			AudioTrack track = ((AudioTrack)(GameGlobals.Songs[dlcName]));
			track.purchased = true;
			GameGlobals.Songs[dlcName] = track;
			DownloadDLCSong(shortCode);
			return true;
		}
		else
			return false;
		/**/
		
	}
	public void DownloadDLCSong(string shortCode)
	{
		
		StartCoroutine("DownloadBundle", shortCode);
	}
	IEnumerator DownloadBundle(string currentBundle)
	{
		Hashtable table = GSApi.getDownloadURL(currentBundle);//get url of dlc
		www = new WWW((string)table["url"]);
		PlayerPrefs.SetInt("DLCComplete", 0);
		yield return www;
		if(www.error == null)
		{
			if (Debug.isDebugBuild) Debug.Log("download Complete");
			
			AssetBundle bundle = www.assetBundle;
			
			if (Debug.isDebugBuild) Debug.Log("saving");
			FileStream fs=new FileStream(Application.persistentDataPath+"/"+currentBundle+".unity3d", FileMode.Create);
			BinaryWriter w = new BinaryWriter(fs);
			w.Write(www.bytes);
			w.Close();
			fs.Close();
			PlayerPrefs.SetInt("DLCComplete", 1);
			bundle.Unload(true);
			
		}
		
	}
}

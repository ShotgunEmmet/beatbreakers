using UnityEngine;
using System.Collections;

public static class GameGlobals
{
	public static bool online = false;
	
	public enum GameState {Journey = 0, AgainstTheClock, Challenge};
	public static GameState gameState = GameState.Journey;
	
	public static bool loading = false;
	public static bool sound = true;
	public static bool messages = true;
	public static int messageCount = 0;
	
	public static int songSelection = 0;
	public static int currentScore = 0;
	
	public static float timeRemaining = 0f;
	
	public static float MultiplierDuration = 0;
	
	//public static string shortCode = "";
	public static string relevantID = "";
	
	public static System.Collections.ArrayList otherIDs;
	public static string lastScene = "";
	
	public static bool gamePaused = false;
	
	public static int challengeAttemptsRemaining = 0;
	
	
	public static int opponentsScore = -1;
	
	public static int challengeScore1 = 10000;
	public static int challengeScore2 = 20000;
	public static int challengeScore3 = 30000;
	public static string userID ="";
	public static Hashtable Songs;
	public static Hashtable TileSets;
	
	public static AudioTrack selectedTrack;
	
	public static TileSet selectedTiles;
	
	public static bool tilesLoaded = false;
	public static Texture2D[] TileImages;
	
	public static void LoadTileImages(){
		TileImages = new Texture2D[16];
		for(int counter = 0; counter < 16; counter++){
			string letter;
			if(counter/4 == 0)
				letter = "a";
			else if(counter/4 == 1)
				letter = "b";
			else if(counter/4 == 2)
				letter = "c";
			else //if(counter/4 == 3)
				letter = "d";
			
			TileImages[counter] = Resources.Load(GameGlobals.selectedTiles.path+((counter%4)+1).ToString()+letter, typeof(Texture2D)) as Texture2D;
			GameGlobals.tilesLoaded = true;
		}
	}
}
public struct AudioTrack
{
	public bool dlc;
	public string song;
	public string albumArt;
	public bool purchased;
	public string challengeShortCode;
	public int inGameCurrencyCost;
	public int gamesparkCurrencyCost;
	public AudioTrack(string song,string albumArt, bool purchased, bool dlc, string challengeShortCode, int inGameCurrencyCost = -1, int gamesparkCurrencyCost = -1){
		this.song = song;
		this.albumArt = albumArt;
		this.purchased = purchased;
		this.challengeShortCode = challengeShortCode;
		this.dlc = dlc;
		this.inGameCurrencyCost = inGameCurrencyCost;
		this.gamesparkCurrencyCost = gamesparkCurrencyCost;
	}
}
public struct TileSet
{
	public string name;
	public string path;
	public bool purchased;
	public string challengeShortCode;
	public TileSet(string name, string path, bool purchased, string challengeShortCode){
		this.name = name;
		this.path = path;
		this.purchased = purchased;
		this.challengeShortCode = challengeShortCode;
	}
}
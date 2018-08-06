using UnityEngine;

using System.Collections;

public class AudioSelection : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip[] audioClip;
	//public float[] spectrumData = new float[4];
	//public float[] outputData = new float[256];
	float[] samples = new float[256];
	float[] curValues = new float[8];
    int count=0;
   	float diff = 0;
	public TileArea tileArea;
	//public Bloom bloom;
	public GlowEffect bloom;
	// Use this for initialization
	float[] highest = new float[4]; 
	bool started = false;
	
	void Start () {
		StartCoroutine("LoadSong");
	}
	
	// Update is called once per frame
	void Update () {
		if(started)
		{
			if(GameGlobals.gamePaused){
				if(audioSource.isPlaying){
					GameGlobals.timeRemaining-=Time.deltaTime;
				}
				audioSource.Pause();
			}
			else{
				if(!audioSource.isPlaying && GameGlobals.timeRemaining > 1.5f){
					audioSource.Play();
				}
				else{
					GameGlobals.timeRemaining-=Time.deltaTime;
				}
			}
		
		}
		curValues = new float[8];
     	count=0;
    	diff = 0;
    	audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
     
     

    	for (int i=0;i<8;++i) 
		{
        	float average = 0;
     
        	int sampleCount = (int) Mathf.Pow(2,i);
        	for (int j=0;j<sampleCount ;++j) 
			{
            	average += samples[count] * (count+1);
            	++count;        
        	}
        	average/=sampleCount;
        	diff = Mathf.Clamp(average * 10 - curValues[i],0,4);
			
        	curValues[i] = average * 10;
    	}
		if(GameGlobals.gamePaused){
			bloom.glowTint = new Color(Mathf.Clamp(curValues[1]/4f,0.3f,.6f), Mathf.Clamp(curValues[3]/4f,0.3f,.6f), Mathf.Clamp(curValues[5]/4f,0.3f,.6f), 1f);
		}
		else if(GameGlobals.timeRemaining <= 0){
			bloom.glowTint = new Color(.1f, .1f, .1f, 1f);
		}
		else{
			bloom.glowTint = new Color(Mathf.Clamp(curValues[1]/4f,0.3f,1f), Mathf.Clamp(curValues[3]/4f,0.3f,1f), Mathf.Clamp(curValues[5]/4f,0.3f,1f), 1f);
		}
		
	}
	
		IEnumerator LoadSong(){
		GameGlobals.timeRemaining = 100f;
		if(GameGlobals.selectedTrack.dlc && GameGlobals.selectedTrack.purchased)
		{
			WWW www;
			if (Debug.isDebugBuild) Debug.Log("file://"+ Application.dataPath+"/Resources/"+GameGlobals.selectedTrack.challengeShortCode.Replace("TKHS","TK")+".unity3d");
			www = new WWW("file://"+ Application.dataPath+"/Resources/"+GameGlobals.selectedTrack.challengeShortCode.Replace("TKHS","TK")+".unity3d");
					
			yield return www;
			AssetBundle bundle = www.assetBundle;
			if (Debug.isDebugBuild) Debug.Log("loading song");
			audioSource.clip = (bundle.mainAsset as AudioClip);
			bundle.Unload(false);

		}
		else if(!GameGlobals.selectedTrack.dlc)
		{
			audioSource.clip = Resources.Load(GameGlobals.selectedTrack.song, typeof(AudioClip)) as AudioClip;
			if (Debug.isDebugBuild) Debug.Log(GameGlobals.selectedTrack.song);
		}
		else
		{
			if (Debug.isDebugBuild) Debug.LogError("no song here");
			yield break;
		}
		started = true;
		GameGlobals.challengeScore1 = (int)audioSource.clip.length * 75;
		GameGlobals.challengeScore2 = (int)audioSource.clip.length * 125;
		GameGlobals.challengeScore3 = (int)audioSource.clip.length * 175;
		
		GameGlobals.timeRemaining = audioSource.clip.length;
		audioSource.Play();
	}
	
	
}

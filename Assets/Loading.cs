using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	
	public Texture2D[] frames;
	private int currentFrame = 0;
	private float delay = .01f;
	
	// Use this for initialization
	void Start () {
		GameGlobals.loading = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(GameGlobals.loading){
			GameGlobals.loading = false;
			
			//Camera.main.cullingMask = ~(0);
			//Camera.main.cullingMask = 0 | 1 << 8;
			
			foreach(Camera cam in FindObjectsOfType(typeof(Camera))){
				cam.cullingMask = 0 | 1 << 9;
				StartCoroutine(AnimateLoading());
			}
		}
	}
	
	IEnumerator AnimateLoading(){
		while(true){
			if(renderer != null)
				renderer.material.mainTexture = frames[currentFrame%frames.Length];
			
			yield return new WaitForSeconds(delay);
			
			currentFrame++;
			//currentFrame%=frames.Length;
			
			if(currentFrame == frames.Length){
				break;
			}
		}
	}
}

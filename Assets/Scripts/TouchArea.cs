using UnityEngine;
using System.Collections;

public class TouchArea : MonoBehaviour {
	public int x, y = 0;
	public Texture2D[] frames;
	private int currentFrame = 0;
	private float delay = .1f;
	
	// Use this for initialization
	void Start () {
		//StartCoroutine(AnimateTouchArea());
		renderer.material.mainTexture = frames[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator AnimateTouchArea(){
		while(true){
			renderer.material.mainTexture = frames[currentFrame];
			
			yield return new WaitForSeconds(delay);
			
			currentFrame++;
			
			if(currentFrame == frames.Length){
				currentFrame%=frames.Length;
				renderer.material.mainTexture = frames[currentFrame];
				break;
			}
		}
	}
	
	public void Touched(){
		StartCoroutine(AnimateTouchArea());
	}
	
}

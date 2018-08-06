using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {
	public float pause = 1f;
	public float duration = 1f;
	public string scene = "";
	float timePassed = 0f;
	
	public Texture2D[] animationFrames;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(SplashAnimation());
	}
	
	// Update is called once per frame
	void Update () {
		/*timePassed+=Time.deltaTime;
		if(timePassed>duration)
		{
			Application.LoadLevel(scene);
		}*/
	}
	
	IEnumerator SplashAnimation(){
		renderer.enabled =false;
		yield return new WaitForSeconds(pause);
		renderer.enabled =true;
		foreach(Texture2D currentFrame in animationFrames){
			renderer.material.mainTexture = currentFrame;
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(duration - ( ((float)animationFrames.Length) * .05f ) - pause);
		Application.LoadLevel(scene);
	}
}

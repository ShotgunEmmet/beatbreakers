using UnityEngine;
using System.Collections;

public class LoadNextScene : MonoBehaviour {
	
	public float waitDuration = 2f;
	
	void Start(){
		StartCoroutine(CountdownToLoad());
	}
	
	void OnClick(){
		Application.LoadLevelAsync(Application.loadedLevel+1);
	}
	
	IEnumerator CountdownToLoad(){
		yield return new WaitForSeconds(waitDuration);
		OnClick();
	}
}

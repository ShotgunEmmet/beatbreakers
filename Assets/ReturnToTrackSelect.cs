using UnityEngine;
using System.Collections;

public class ReturnToTrackSelect : MonoBehaviour {

	public GameObject[] trackSelection;
	public GameObject[] notTrackSelection;
	
	void OnClick(){
		foreach(GameObject current in trackSelection)
			current.SetActive(true);
		foreach(GameObject current in notTrackSelection)
			current.SetActive(false);
		transform.parent.gameObject.SetActive(false);
	}
}

using UnityEngine;
using System.Collections;

public class Online : MonoBehaviour {

	public GameObject[] online, offline;
	
	
	void Start () {
		if (GameGlobals.online){
			foreach(GameObject current in online)
				current.SetActive(true);
			foreach(GameObject current in offline)
				current.SetActive(false);
		}
		else{
			foreach(GameObject current in online)
				current.SetActive(false);
			foreach(GameObject current in offline)
				current.SetActive(true);
		}
			
	}
	
}

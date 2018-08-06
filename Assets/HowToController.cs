using UnityEngine;
using System.Collections;

public class HowToController : MonoBehaviour {

	public HowToScreen[] screens;
	private int currentScreen;
	
	// Use this for initialization
	void Start () {
		for(int counter = 1; counter < screens.Length; counter++){
			foreach(GameObject current in screens[counter].screen){
				current.SetActive(false);
			}
		}
		foreach(GameObject current in screens[currentScreen].screen){
			current.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Next(){
		foreach(GameObject current in screens[currentScreen].screen){
			current.SetActive(false);
		}
		currentScreen++;
		currentScreen%=screens.Length;
		foreach(GameObject current in screens[currentScreen].screen){
			current.SetActive(true);
		}
	}
}

[System.Serializable ]
public class HowToScreen
{
	public GameObject[] screen;
}

using UnityEngine;
using System.Collections;

public class PressedButton : MonoBehaviour {
	
	public bool pressed = false;
	
	void OnClick(){
		pressed = true;
	}
}

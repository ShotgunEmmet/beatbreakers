using UnityEngine;
using System.Collections;

public class HowToNextButton : MonoBehaviour {
	
	public HowToController howTo;
	
	void OnClick(){
		howTo.Next();
	}
}

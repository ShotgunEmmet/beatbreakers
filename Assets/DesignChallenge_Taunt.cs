using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]

public class DesignChallenge_Taunt : MonoBehaviour {
	
	UIInput mInput;
	
	void Start ()
	{
		mInput = GetComponent<UIInput>();
	}
	
	public void OnSubmit()
	{
		string text = NGUITools.StripSymbols(mInput.text);

		if (!string.IsNullOrEmpty(text))
		{
			//post taunt username   + ": "+text   to the challenge chat online
			
		}
	}
	
}
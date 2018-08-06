using UnityEngine;
using System.Collections;

public class ChallengeChatSend : MonoBehaviour {
	
	public Challenge_Chat chat;
	
	void OnClick(){
		chat.InputText();
		chat.OnSubmit();
	}
	
}

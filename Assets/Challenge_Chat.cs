using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]

public class Challenge_Chat : MonoBehaviour 
{
	public UITextList textList;
	public bool fillWithDummyData = false;

	UIInput mInput;
	bool mIgnoreNextEnter = false;


	
	bool whiteGreySwitch = true;
	GameSparks GSApi; 
	
	string callbackString = "";
	void Start ()
	{
		
				
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		//GSApi.listChallengeDetails(GameGlobals.relevantID);
		
		
		
		GSApi.GSMessageReceived += (GS, args)=>
		{
			 Debug.Log("message recieved");
			if(args.Message.ContainsKey("challenge"))
			{
				 Debug.Log("adding line");
				if((string)(( (Hashtable)args.Message["challenge"] )["challengeId"]) == GameGlobals.relevantID )
				{
					 Debug.Log((string)args.Message["who"]);
					 Debug.Log((string)args.Message["message"]);
					callbackString= (whiteGreySwitch ? "[FFFFFF]" : "[AAAAAA]") +(string)args.Message["who"]+ ": " + (string)args.Message["message"];
					whiteGreySwitch = !whiteGreySwitch;
				}
			}
		};

		
		mInput = GetComponent<UIInput>();

		ArrayList messages = GSApi.GetMessages()["messageList"] as ArrayList;
		if(messages!= null)
		{
			foreach(Hashtable message in messages)
			{
				if(((string)message["@class"] ==".ChallengeChatMessage") )
				{
					if(  ( (string)( (Hashtable)message["challenge"] )["challengeId"] )== GameGlobals.relevantID  )
					{
						 Debug.Log(message["message"]);
					
						textList.Add((whiteGreySwitch ? "[FFFFFF]" : "[AAAAAA]") +
							((string)message["who"])+ ": " + ((string)message["message"]));
						whiteGreySwitch = !whiteGreySwitch;
					}
				}
			}
		}

		
	}

	/// <summary>
	/// Pressing 'enter' should immediately give focus to the input field.
	/// </summary>

	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Return))
		{
			InputText();
		}
		if(callbackString!= "")
		{
			
			AddFromCallback(callbackString);
			callbackString = "";
		}
	}
	void AddFromCallback(string text)
	{
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			

			if (!string.IsNullOrEmpty(text))
			{
				textList.Add(text);
				whiteGreySwitch = !whiteGreySwitch;
			}
		}
		mIgnoreNextEnter = true;
	}
	public void InputText(){
		if (!mIgnoreNextEnter && !mInput.selected)
			{
				mInput.selected = true;
			}
			mIgnoreNextEnter = false;
	}
	
	/// <summary>
	/// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>

	public void OnSubmit()
	{
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUITools.StripSymbols(mInput.text);

			if (!string.IsNullOrEmpty(text))
			{
				//textList.Add((whiteGreySwitch ? "[FFFFFF]" : "[AAAAAA]") + "Me: " + text);
				GSApi.chatOnChallenge(GameGlobals.relevantID, text);
				
				//post username   + ": "+text   to the challenge chat online
				mInput.text = "";
				mInput.selected = false;
				whiteGreySwitch = !whiteGreySwitch;
			}
		}
		mIgnoreNextEnter = true;
	}
}
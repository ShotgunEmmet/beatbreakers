using UnityEngine;
using System.Collections;

public class Scroll_Item_FriendsList : MonoBehaviour {
	
	public string id;
	public UILabel number;
	public UILabel name;
	public GameObject tickBoxChecked;
	public GameObject tickBoxUnchecked;
	
	public static int itemCount = 1;
	
	void Start()
	{
		transform.parent.name = itemCount.ToString("00");
		number.text = itemCount.ToString("00");
		itemCount++;
	}
	
	void OnClick()
	{
		tickBoxChecked.SetActive(!tickBoxChecked.activeSelf);
		tickBoxUnchecked.SetActive(tickBoxUnchecked.activeSelf);
		if(tickBoxChecked.activeSelf)
			GameGlobals.otherIDs.Add(id);
		else
			GameGlobals.otherIDs.Remove(id);
	}
		
}

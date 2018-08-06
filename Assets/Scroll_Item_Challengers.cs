using UnityEngine;
using System.Collections;

public class Scroll_Item_Challengers : MonoBehaviour {

	public string id;
	public UILabel name;
	public UILabel topScore;
	public UILabel attempts;
	
	public static int itemCount = 1;
	
	void Start()
	{
		transform.parent.name = itemCount.ToString("00");
		itemCount++;
	}
	
	
}
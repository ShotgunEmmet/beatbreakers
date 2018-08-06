using UnityEngine;
using System.Collections;

public class Scroll_Item_Challengers_Finished : MonoBehaviour {
	
	public string id;
	public UILabel position;
	public UILabel name;
	public UILabel topScore;
	
	public static int itemCount = 1;
	
	void Start()
	{
		transform.parent.name = itemCount.ToString("00");
		itemCount++;
	}
	
	
}
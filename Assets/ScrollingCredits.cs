using UnityEngine;
using System.Collections;

public class ScrollingCredits : MonoBehaviour {

	public float speed = 1f;
	
	void Update () {
		transform.Translate(Vector3.up*speed*Time.deltaTime);
		if(transform.localPosition.y > 20200)
			transform.localPosition = new Vector3(0,-2000,0);
	}
}

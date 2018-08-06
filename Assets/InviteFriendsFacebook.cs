using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InviteFriendsFacebook : MonoBehaviour {

	void OnClick(){
		var options = new Dictionary<string, string>();
		options["message"] = "Play BeatBreakers with me";
		FacebookAndroid.showDialog("apprequests", options);
	}
}

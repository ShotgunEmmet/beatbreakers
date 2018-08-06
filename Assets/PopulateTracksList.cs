using UnityEngine;
using System.Collections;
using System.Linq;
public class PopulateTracksList : MonoBehaviour {
	
	//public GameObject trackPrefab;
	public GameObject trackPrefab;
	
	// Use this for initialization
	void Start () {
		
		for(int counter = transform.childCount-1; counter >= 0; counter-- ){
			Destroy(transform.GetChild(counter).gameObject);
		}
		
		int count =0;
		var keys = GameGlobals.Songs.Keys.Cast<string>().OrderBy(x => x);
		foreach(string key in keys)
		{
			GameObject item = Instantiate(trackPrefab,trackPrefab.transform.position,Quaternion.identity) as GameObject;
			item.transform.parent = this.transform;
			item.name = count.ToString("00");
			item.transform.localScale = new Vector3(1,1,1);
			item.transform.localPosition = item.transform.position + new Vector3(0,-46*count,0);
			
			(item.GetComponentInChildren(typeof(TrackStoreItem)) as TrackStoreItem).SetupTrack((AudioTrack)GameGlobals.Songs[key]);
			
			count++;
		}
		/*
		for(int counter = 0; counter < GameGlobals.Songs.Count; counter++){
				
				GameObject item = Instantiate(trackPrefab,trackPrefab.transform.position,Quaternion.identity) as GameObject;
				item.transform.parent = this.transform;
				item.name = counter.ToString("00");
				item.transform.localScale = new Vector3(1,1,1);
				item.transform.localPosition = item.transform.position + new Vector3(0,-46*counter,0);
			
				(item.GetComponentInChildren(typeof(TrackStoreItem)) as TrackStoreItem).SetupTrack((AudioTrack)GameGlobals.Songs[counter]);
				/*
				(item.GetComponentInChildren(typeof(TrackStoreItem)) as TrackStoreItem).label.text = ((AudioTrack)GameGlobals.Songs[counter]).song;
				//(item.GetComponentInChildren(typeof(TrackStoreItem)) as TrackStoreItem).albumArt = ((AudioTrack)GameGlobals.Songs[counter]).albumArt;
				(item.GetComponentInChildren(typeof(TrackStoreItem)) as TrackStoreItem).unlocked = ((AudioTrack)GameGlobals.Songs[counter]).purchased;
				*/
			/*
			
		}*/
		Vector4 scrolArea = ((UIPanel)transform.parent.GetComponent(typeof(UIPanel))).clipRange;
		scrolArea.w = 675 * GameGlobals.Songs.Count + /*480*/500*(GameGlobals.Songs.Count-1);
		//900*first 3 then 1800*rest-1
		((UIPanel)transform.parent.GetComponent(typeof(UIPanel))).clipRange = scrolArea;
		
		//((UIPanel)transform.parent.GetComponent(typeof(UIPanel))).clipRange.y = 1000 * GameGlobals.Songs.Count + 1500;
		// Debug.LogError(((UIPanel)transform.parent.GetComponent(typeof(UIPanel))).clipRange.y .ToString());
		// Debug.LogError(((UIPanel)transform.parent.GetComponent(typeof(UIPanel))).clipRange.w .ToString());
		
		((UIGrid)GetComponent(typeof(UIGrid))).repositionNow = true;
	}
}

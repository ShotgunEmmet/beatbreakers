using UnityEngine;
using System.Collections;

public class PopulateSkinsList : MonoBehaviour {
	UIPopupList popupMenu;
	string oldValue;
	
	
	// Use this for initialization
	void Start () {
		popupMenu = ( (UIPopupList) GetComponent(typeof(UIPopupList)) );
		popupMenu.items.Clear();
		foreach(DictionaryEntry skin in GameGlobals.TileSets){
			
			popupMenu.items.Add(((TileSet)skin.Value).name.ToString());
			
			if( GameGlobals.selectedTiles.name.Contains(((TileSet)skin.Value).name.ToString()) )
				popupMenu.selection = ((TileSet)skin.Value).name.ToString();
		}
		
		oldValue = popupMenu.selection;
		
	}
	
	void Update(){
		if(oldValue != popupMenu.selection){
			oldValue = popupMenu.selection;
			
			foreach(DictionaryEntry skin in GameGlobals.TileSets){
				if( ((TileSet)skin.Value).name.ToString().Contains(oldValue) ){
					PlayerPrefs.SetString("TileSet", oldValue);
					GameGlobals.tilesLoaded = false;
					GameGlobals.selectedTiles = ((TileSet)skin.Value);
				}
			}
			
			
		}
		
	}
}

using UnityEngine;
using System.Collections;

public class BuyTrackInGameCurrency : MonoBehaviour {

	public UILabel price;
	
	public GameObject ConfirmationBackground;
	public UILabel question;
	public PressedButton yes, no;
	private bool confirmation = false;
	
	void Start () {
		ShowConfirmation(false);
		
	}
	void OnClick(){
		confirmation = true;
		ShowConfirmation(true);
		question.text = "Are you sure?";
	}
	// Update is called once per frame
	void Update () {
		//price = GameGlobals.selectedTrack.inGameCurrencyCost.ToString();
		if(confirmation){
			if(yes.pressed){
				yes.pressed = false;
				confirmation = false;
				
				if(PlayerPrefs.GetInt("InGameCurrency") < GameGlobals.selectedTrack.inGameCurrencyCost){
					StartCoroutine(InsufficentFunds());
				}
				else{
					//Pierce/ Unlock song & download
					PlayerPrefs.SetInt("InGameCurrency", PlayerPrefs.GetInt("InGameCurrency")-GameGlobals.selectedTrack.inGameCurrencyCost);
					
					GameGlobals.loading = true;
					Application.LoadLevelAsync("BuyTracks");
				}
			}
			else if(no.pressed){
				no.pressed = false;
				confirmation = false;
				ShowConfirmation(false);
			}
		}
	}
	
	void ShowConfirmation(bool show){
		ConfirmationBackground.SetActive(show);
		question.gameObject.SetActive(show);
		yes.gameObject.SetActive(show);
		no.gameObject.SetActive(show);
	}
	
	IEnumerator InsufficentFunds(){
		yes.gameObject.SetActive(false);
		no.gameObject.SetActive(false);
		question.text = "INSUFFICIENT\nFUNDS";
		yield return new WaitForSeconds(1f);
		ShowConfirmation(false);
	}
}

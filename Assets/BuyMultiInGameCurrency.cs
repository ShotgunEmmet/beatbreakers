using UnityEngine;
using System.Collections;

public class BuyMultiInGameCurrency : MonoBehaviour {

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
		if(confirmation){
			if(yes.pressed){
				yes.pressed = false;
				confirmation = false;
				
				if(PlayerPrefs.GetInt("InGameCurrency") < 500){
					StartCoroutine(InsufficentFunds());
				}
				else{
					PlayerPrefs.SetInt("X2s", PlayerPrefs.GetInt("X2s")+3);
					PlayerPrefs.SetInt("InGameCurrency", PlayerPrefs.GetInt("InGameCurrency")-500);
					
					GameGlobals.loading = true;
					Application.LoadLevelAsync("BuyMultipliers");
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

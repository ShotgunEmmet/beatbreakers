using UnityEngine;
using System.Collections;

public class BuyCoins : MonoBehaviour {
	public string sku = "";
	
	public GameObject ConfirmationBackground;
	public UILabel question;
	public PressedButton yes, no;
	private bool confirmation = false;
	
	//public GameSparks GSApi;
	// Use this for initialization
	void Start () {
		ShowConfirmation(false);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(confirmation){
			if(yes.pressed){
				yes.pressed = false;
				confirmation = false;
				
#if !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN
				IAP.purchaseConsumableProduct( sku, didSucceed =>
				{
					 Debug.Log( "purchasing product " + sku + " result: " + didSucceed );
				});
				
				//GoogleIAB.purchaseProduct(sku);
#endif
				ShowConfirmation(false);
				
			}
			else if(no.pressed){
				no.pressed = false;
				confirmation = false;
				ShowConfirmation(false);
			}
		}
	}
	void OnClick()
	{
		confirmation = true;
		ShowConfirmation(true);
		question.text = "Are you sure?";

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

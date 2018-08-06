using UnityEngine;
using System.Collections;

public class BuyTrackRealMoney : MonoBehaviour {
	public GameSparks GSApi;
	public UILabel price;
	
	public GameObject ConfirmationBackground;
	public UILabel question;
	public PressedButton yes, no;
	private bool confirmation = false;
	DLCManager dlc;
	// Use this for initialization
	void Start () {
		ShowConfirmation(false);
		dlc = GameObject.FindObjectOfType(typeof(DLCManager)) as DLCManager;
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		Hashtable virtualGoodsResponse = GSApi.listVirtualGoods();
		ArrayList virtualGoods = (ArrayList)(virtualGoodsResponse["virtualGoods"]);
		foreach(Hashtable good in virtualGoods)
		{
			if((string)((Hashtable)good)["shortCode"] == GameGlobals.selectedTrack.challengeShortCode.Replace("TKHS", "TK"))
			{
				//price.text =((double)((Hashtable)good)["currency3Cost"]).ToString();
				price.text = GameGlobals.selectedTrack.gamesparkCurrencyCost.ToString();
				break;
			}
		}
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
				
				Hashtable accountDetails = GSApi.accountDetails();
				
				if( ((double)accountDetails["currency3"]) < (double)GameGlobals.selectedTrack.gamesparkCurrencyCost ){
					StartCoroutine(InsufficentFunds());
				}
				else{
					if(dlc.PurchaseTrack(GameGlobals.selectedTrack.song, GameGlobals.selectedTrack.challengeShortCode.Replace("TKHS", "TK"))){
						GameGlobals.loading = true;
						Application.LoadLevelAsync("BuyTracks");
					}
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

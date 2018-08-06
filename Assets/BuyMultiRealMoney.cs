using UnityEngine;
using System.Collections;

public class BuyMultiRealMoney : MonoBehaviour {
	public GameSparks GSApi;
	public UILabel price;
	
	public GameObject ConfirmationBackground;
	public UILabel question;
	public PressedButton yes, no;
	private bool confirmation = false;
	
	// Use this for initialization
	void Start () {
		ShowConfirmation(false);
		
		GSApi = GameObject.FindObjectOfType(typeof(GameSparks)) as GameSparks;
		Hashtable virtualGoodsResponse = GSApi.listVirtualGoods();
		ArrayList virtualGoods = (ArrayList)(virtualGoodsResponse["virtualGoods"]);
		foreach(Hashtable good in virtualGoods)
		{
			if((string)((Hashtable)good)["shortCode"] == "X2M")
			{
				price.text =((double)((Hashtable)good)["currency3Cost"]).ToString();
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
		if(confirmation){
			if(yes.pressed){
				yes.pressed = false;
				confirmation = false;
				
				Hashtable response = GSApi.buyVirtualGoodWithVirtualCurrency("X2M", 1,3);
				if(response["error"] != null){
					if((string)((Hashtable)response["error"])["currency3"]== "INSUFFICIENT_FUNDS"){
						StartCoroutine(InsufficentFunds());
					}
					else{
						//Pierce/ decrement the server currency here (I have this happen first so you can verify the return & do the next line then)
						PlayerPrefs.SetInt("X2s", PlayerPrefs.GetInt("X2s")+3);
						
						GameGlobals.loading = true;
						Application.LoadLevelAsync("BuyMultipliers");
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

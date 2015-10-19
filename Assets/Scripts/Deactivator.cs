using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soomla.Store.IAP
{
public class Deactivator : MonoBehaviour {

	// Use this for initialization
	Button button ;
	public bool isLifeTime;
	public int price;
	
	public string lifeTimeItemId;
	
	void Start () {
		button = this.GetComponent<Button>();	
		
	}
	
	// Update is called once per frame
	void Update () {
		Deactivate(price,isLifeTime,lifeTimeItemId);
	}
	
	public void Deactivate(int price , bool isLifeTime, string lifeTimeItemId){
		if (StoreInventory.GetItemBalance("coin_currency_id")<price)
		{
			button.interactable=false;
		}else{
			button.interactable = true;
			if(isLifeTime){
				if (StoreInventory.GetItemBalance(lifeTimeItemId)==1)
				{
					button.interactable=false;
				}
				
			}
		}
		

	}
}
}

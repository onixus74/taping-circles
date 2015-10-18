﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soomla.Store.IAP
{
public class Coins_Text : MonoBehaviour {

	GameObject gameManager;
	
	void Start () {
	 gameManager = GameObject.FindGameObjectWithTag("game manager");
	}
	
	// Update is called once per frame
	void Update () {
	//  this.GetComponent<Text>().text = ""+gameManager.GetComponent<GameManager>().coins.ToString();
	  this.GetComponent<Text>().text = StoreInventory.GetItemBalance("coin_currency_id").ToString();
	}
}
}

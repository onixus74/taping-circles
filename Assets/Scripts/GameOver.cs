﻿using UnityEngine;
using System.Collections;

namespace Soomla.Store.IAP
{
public class GameOver : MonoBehaviour {
	
	// Use this for initialization\
	 public Animator anim;
	//   public AdmobAdManager admobManager;
	public UnityAdsManager unityAdsManager;
	public CharboostAdsManager charboostAdsManager;
	
	bool isGameOverCounted;

	void Awake () {
		unityAdsManager = GameObject.FindGameObjectWithTag("unityads manager").GetComponent<UnityAdsManager>();
		charboostAdsManager = GameObject.FindGameObjectWithTag("charboostads manager").GetComponent<CharboostAdsManager>();
		anim = this.GetComponent<Animator>();
		//  admobManager = GameObject.FindGameObjectWithTag("game manager").transform.FindChild("AdmobManager").GetComponent<AdmobAdManager>();
		//  admobManager.LoadInterstitialAds();
		 
		isGameOverCounted = false;
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.instance.isGameOver==true)
		{	
			
			
			
			anim.SetBool("check",true);
			//  admobManager.ShowInterstitialAds();
			GameManager.instance.canClick=false;
		
		}
	}
	
	public void gameOverCounter(){
		GameManager.instance.gameOverCounter++;
			
			
			if(GameManager.instance.gameOverCounter>=3){
				GameManager.instance.gameOverCounter=0;
				if (StoreInventory.GetItemBalance("remove_ads_item_id")==0)
				{
					//  unityAdsManager.ShowAds();		
					charboostAdsManager.ShowRewardedVideo();	
				}
			}
			PlayerPrefs.SetInt("gameOverCounter",GameManager.instance.gameOverCounter);
			
	}
	public void playGameOver(){
		AudioManager.instance.GameOver();
	}
}
}

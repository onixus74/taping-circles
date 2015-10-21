using UnityEngine;
using System.Collections;

namespace Soomla.Store.IAP
{
public class GameOver : MonoBehaviour {
	GameManager gameManager;
	// Use this for initialization\
	 public Animator anim;
	//   public AdmobAdManager admobManager;
	public UnityAdsManager unityAdsManager;
	
	bool isGameOverCounted;

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager").GetComponent<GameManager>();
		unityAdsManager = GameObject.FindGameObjectWithTag("unityads manager").GetComponent<UnityAdsManager>();
		anim = this.GetComponent<Animator>();
		//  admobManager = GameObject.FindGameObjectWithTag("game manager").transform.FindChild("AdmobManager").GetComponent<AdmobAdManager>();
		//  admobManager.LoadInterstitialAds();
		 
		isGameOverCounted = false;
	}

	// Update is called once per frame
	void Update () {
		if (gameManager.isGameOver==true)
		{	
			
			
			
			anim.SetBool("check",true);
			//  admobManager.ShowInterstitialAds();
			gameManager.canClick=false;
		
		}
	}
	
	public void gameOverCounter(){
		gameManager.gameOverCounter++;
			
			
			if(gameManager.gameOverCounter>=3){
				gameManager.gameOverCounter=0;
				if (StoreInventory.GetItemBalance("remove_ads_item_id")==0)
				{
					unityAdsManager.ShowAds();					
				}
			}
			PlayerPrefs.SetInt("gameOverCounter",gameManager.gameOverCounter);
			
	}
}
}

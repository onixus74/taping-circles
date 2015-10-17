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

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager").GetComponent<GameManager>();
		unityAdsManager = GameObject.FindGameObjectWithTag("unityads manager").GetComponent<UnityAdsManager>();
		anim = this.GetComponent<Animator>();
		//  admobManager = GameObject.FindGameObjectWithTag("game manager").transform.FindChild("AdmobManager").GetComponent<AdmobAdManager>();
		//  admobManager.LoadInterstitialAds();
		 
		 gameManager.gameOverCounter=PlayerPrefs.GetInt("GameOverCounter");
		 print(PlayerPrefs.GetInt("GameOverCounter"));
		
	}
	void OnLevelWasLoaded(int level) {
        if (level == 1){
			gameManager.gameOverCounter++;
			if(gameManager.gameOverCounter==3){
				unityAdsManager.ShowAds();
				gameManager.gameOverCounter=1;
			}
		}
            
        
    }
	// Update is called once per frame
	void Update () {
		if (gameManager.isGameOver==true)
		{
			anim.SetBool("check",true);
			PlayerPrefs.SetInt("GameOverCounter",gameManager.gameOverCounter);
			//  admobManager.ShowInterstitialAds();
			
		}
	}
}
}

using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	GameObject gameManager;
	// Use this for initialization\
	 public Animator anim;
	//   public AdmobAdManager admobManager;

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager");
		anim = this.GetComponent<Animator>();
		//  admobManager = GameObject.FindGameObjectWithTag("game manager").transform.FindChild("AdmobManager").GetComponent<AdmobAdManager>();
		//  admobManager.LoadInterstitialAds();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.GetComponent<GameManager>().isGameOver==true)
		{
			anim.SetBool("check",true);
			//  admobManager.ShowInterstitialAds();

		}
	}
}

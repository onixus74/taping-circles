using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Soomla.Store.IAP
{
public class PanelManager : MonoBehaviour {

	// Use this for initialization
	public GameObject gamePanel;
	public GameObject gameOverPanel;
	public GameObject shopPanel;
	public GameObject test;
	
	Animator animGameOver;
	Animator animShop;
	GameManager gameManager;
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager").GetComponent<GameManager>();
		gamePanel = GameObject.Find("Game_Panel");
		shopPanel = GameObject.Find("Shop_Panel");
		gameOverPanel = GameObject.FindGameObjectWithTag("gameover_panel");
		
		animGameOver = gameOverPanel.GetComponent<Animator>();
		animShop = shopPanel.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameManager.isGameOver){
			animGameOver.SetBool("isShowing",true);
		}
	}
	
	public void showShopPanel(){
		animShop.SetBool("isShowing",true);
	}
	
	public void hideShopPanel(){
		animShop.SetBool("isShowing",false);
	}
}
}

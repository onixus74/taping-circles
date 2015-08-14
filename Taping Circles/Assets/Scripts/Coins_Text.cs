using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coins_Text : MonoBehaviour {

	GameObject gameManager;
	
	void Start () {
	 gameManager = GameObject.FindGameObjectWithTag("game manager");
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Text>().text = "Coins: "+gameManager.GetComponent<GameManager>().coins.ToString();
	}
}

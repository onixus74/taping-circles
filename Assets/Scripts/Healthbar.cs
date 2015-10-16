using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Soomla.Store.IAP
{
public class Healthbar : MonoBehaviour {

	// Use this for initialization
	GameObject gameManager;
	int health;
	
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager");
	}
	
	// Update is called once per frame
	void Update () {
		health= (int)gameManager.GetComponent<GameManager>().health;
		this.GetComponent<Text>().text = health.ToString();
	
	}
}
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soomla.Store.IAP
{
public class HighestLevel : MonoBehaviour {
	
	GameManager gameManager;
	// Use this for initialization
	void Start () {
	gameManager = GameObject.FindGameObjectWithTag("game manager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Text>().text = ""+gameManager.levelShow.ToString();
	}
}
}

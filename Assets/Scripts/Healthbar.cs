using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Soomla.Store.IAP
{
public class Healthbar : MonoBehaviour {

	// Use this for initialization
	int health;
	
	
	// Update is called once per frame
	void Update () {
		health= (int)GameManager.instance.GetComponent<GameManager>().health;
		this.GetComponent<Text>().text = health.ToString();
	
	}
}
}
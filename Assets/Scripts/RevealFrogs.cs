using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soomla.Store.IAP
{
public class RevealFrogs : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	this.GetComponent<Text>().text = "x"+StoreInventory.GetItemBalance("reveal_frogs").ToString();
	}
}
}

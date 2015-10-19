using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soomla.Store.IAP
{
public class ResetLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 this.GetComponent<Text>().text = "x"+StoreInventory.GetItemBalance("reset_level").ToString();
	}
}
}

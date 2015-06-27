using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Healthbar : MonoBehaviour {

	// Use this for initialization
	GameObject gameManager;
	Image image;
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager");
		image=this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		image.fillAmount=gameManager.GetComponent<GameManager>().health;
	}
}

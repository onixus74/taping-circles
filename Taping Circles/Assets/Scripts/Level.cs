using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Level : MonoBehaviour {

	// Use this for initialization
	GameObject gameManager;
	void Start () {
	gameManager = GameObject.FindGameObjectWithTag("game manager");
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Text>().text=""+gameManager.GetComponent<GameManager>().levelShow;
	}
}

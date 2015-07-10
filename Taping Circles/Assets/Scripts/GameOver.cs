using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	GameObject gameManager;
	// Use this for initialization\
	 public Animator anim;

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager");
		anim = this.GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.GetComponent<GameManager>().isGameOver==true)
		{
			anim.SetBool("check",true);

		}
	}
}

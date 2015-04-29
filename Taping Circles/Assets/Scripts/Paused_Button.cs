using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

public class Paused_Button : MonoBehaviour {
	public bool paused = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void pause () {

		paused = !paused ;
		Time.timeScale = paused ? 0 : 1 ;

		/*Time.timeScale = 0;
		paused = true;*/
	}
}

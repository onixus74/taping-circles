using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Restart_Button : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void restart() {

		Application.LoadLevel (Application.loadedLevel);
		//GUI.color=Color(1.0,1.0,1.0,1.0);
	}
}

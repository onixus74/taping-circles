using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Restart_Button : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	private void OnGUI() {
		if (GUI.Button (new Rect (15, 540, 200, 70), "Load Level"))
		Application.LoadLevel (Application.loadedLevel);
		//GUI.color=Color(1.0,1.0,1.0,1.0);
	}
}

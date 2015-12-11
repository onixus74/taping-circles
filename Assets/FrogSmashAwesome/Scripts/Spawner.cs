using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public GameObject BugPrefab;

	void OnGUI(){
		if (GUI.Button (new Rect (0, 0, 100, 50), "Spawn Bug")) {
			Instantiate(BugPrefab);
		}
	}
}

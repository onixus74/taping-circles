using UnityEngine;
using System.Collections;

public class CircleBehaviour : MonoBehaviour {


    float rScreen = 40f;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            SpawnRandom();
        }    	
	}



    public void SpawnRandom()
    {
       

        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(rScreen, Screen.width-rScreen-100), Random.Range(rScreen, Screen.height-rScreen), Camera.main.farClipPlane / 2));
       

        this.gameObject.transform.position = screenPosition;

    }
}

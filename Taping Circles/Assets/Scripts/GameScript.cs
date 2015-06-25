using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	//  public GameObject spawner = GameObject.Find("Spawner");
	public GameObject circle=GameObject.Find("circle");
	void Start () {
		int count=0;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 7; j++)
			{	
				count++;
				GameObject tmpObj=Instantiate(Resources.Load("Spawner",typeof(GameObject)))as GameObject; 
				tmpObj.name=count.ToString();
				tmpObj.transform.position = new Vector3(j*1.5f-5.25f,i*-1.5f+3,9);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public bool paused = false;
    public GameObject ball;
   // public Transform proxyBall;
    
    public int fixedNumber=5;
    public int ballNumber=5;
    public int current = 0;


    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < fixedNumber; i++)
        {
            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(40, Screen.width - 40 - 100), Random.Range(40, Screen.height - 40), Camera.main.farClipPlane / 2));
           // GameObject proxyBall=Instantiate(ball,screenPosition,Quaternion.identity) as GameObject;
           // proxyBall.gameObject.GetComponent<Text>().text = ballNumber.ToString();
            Instantiate(ball, screenPosition, Quaternion.identity);
            
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        current = ballNumber - (fixedNumber - 1);
    }



}

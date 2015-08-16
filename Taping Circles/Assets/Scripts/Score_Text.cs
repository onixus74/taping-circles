using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score_Text : MonoBehaviour
{   int score ; 

    GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("game manager");
    }
    
    

    // Update is called once per frame
    void Update()
        
    {   
        score = (int)gameManager.GetComponent<GameManager>().score;
        this.GetComponent<Text>().text = "Score: " +score.ToString();
    }
}

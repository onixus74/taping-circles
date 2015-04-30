using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircleBehaviour : MonoBehaviour
{


    float rScreen = 40f;
    public Animator anim;
    GameObject gameManager;
    public int ballNBR;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("game manager");
        ballNBR = gameManager.GetComponent<GameManager>().ballNumber;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            gameObject.GetComponent<Text>().text = gameManager.GetComponent<GameManager>().ballNumber.ToString();


        }


    }



    public void SpawnRandom()
    {


        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(rScreen, Screen.width - rScreen - 100), Random.Range(rScreen, Screen.height - rScreen), Camera.main.farClipPlane / 2));

        anim.SetTrigger("spawn");
        this.gameObject.transform.position = screenPosition;
        this.gameObject.GetComponentInChildren<Text>().text = gameManager.GetComponent<GameManager>().ballNumber.ToString();


    }

    public void OnMouseDown()
    {
        anim.SetTrigger("wipe");

        gameManager.GetComponent<GameManager>().ballNumber++;
      
        
    }




}

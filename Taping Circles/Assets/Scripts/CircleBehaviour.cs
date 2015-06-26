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

    }


    // Update is called once per frame
    void Update()
    {



    }


    public void OnMouseDown()
    {
        if ( this.gameObject.name.Equals("ball_" + gameManager.GetComponent<GameManager>().current.ToString()))
        {
            anim.SetTrigger("wipe");
            gameManager.GetComponent<GameManager>().current++;
            Destroy(this.gameObject, 1);
        }
        else
        Debug.Log("GAME OVER");

    }



}

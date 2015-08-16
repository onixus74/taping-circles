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
        if (gameManager.GetComponent<GameManager>().isGameOver == true)
        {
            anim.SetTrigger("wipe");
        }


    }


    public void OnMouseDown()
    {
        if (gameManager.GetComponent<GameManager>().current == gameManager.GetComponent<GameManager>().startNumber)
        {
            gameManager.GetComponent<GameManager>().Hide();
            gameManager.GetComponent<GameManager>().coins += (gameManager.GetComponent<GameManager>().current);
        }
        if (this.gameObject.name.Equals("ball_" + gameManager.GetComponent<GameManager>().current.ToString()))
        {
            anim.SetTrigger("wipe");
            gameManager.GetComponent<GameManager>().current++;
            gameManager.GetComponent<GameManager>().coins += (gameManager.GetComponent<GameManager>().current - 1) * 2;
            Destroy(this.gameObject, 1);
        }
        else
        {
            Debug.Log("GAME OVER");
            anim.SetTrigger("mistake");
            gameManager.GetComponent<GameManager>().health = gameManager.GetComponent<GameManager>().health - (1.0f / gameManager.GetComponent<GameManager>().seqNumber);
        }
    }

    void OrientationRotation()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            this.transform.FindChild("Canvas").FindChild("Text").transform.Rotate(new Vector3(0, 0, 90));
        }

        if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            this.transform.FindChild("Canvas").FindChild("Text").transform.Rotate(new Vector3(0, 0, -90));
        }
    }

    public void showCircle()
    {
        
        this.transform.GetChild(0).GetComponentInChildren<Text>().enabled = true;
        anim.SetTrigger("spawn");
        Invoke("hideCircle", 2.0f);
        
 
    }

    public void hideCircle()
    {
       anim.SetTrigger("spawn");
        this.transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;
    }

}

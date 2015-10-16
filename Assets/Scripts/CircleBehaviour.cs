using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Soomla.Store.IAP
{
public class CircleBehaviour : MonoBehaviour
{
    public Animator anim;
    GameManager gameManager;
    public int ballNBR;

    Transform HUD;
    Text HUD_text;
    Image HUD_image;

    public AudioSource frogAudioSource;

    public AudioClip[] frogAudioClips;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("game manager").GetComponent<GameManager>();
        HUD = gameObject.transform.FindChild("Canvas").FindChild("HUD");
        HUD_text = HUD.GetComponent<Text>();
        HUD_image = HUD.transform.FindChild("Image").GetComponent<Image>();
        frogAudioSource = this.GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOver == true)
        {
            anim.SetTrigger("wipe");
        }


    }


    public void OnMouseDown()
    {
        if (gameManager.canClick)
        {
            anim.SetTrigger("hud_show");
            if (gameManager.current == gameManager.startNumber)
            {
                gameManager.Hide();
                gameManager.isHideClicked = true;
                //  gameManager.coins += (gameManager.current);

            }
            if (this.gameObject.name.Equals("ball_" + gameManager.current.ToString()))
            {
                anim.SetTrigger("wipe");
                gameManager.current++;
                gameManager.rate++;
                gameManager.coins += 1 * gameManager.rate;
                Destroy(this.gameObject, 1);
                if (gameManager.rate > 4)
                {
                    gameManager.rate = 4;
                }
                gameManager.health = gameManager.health + 1.5f;
                HUD_image.overrideSprite = Resources.Load<Sprite>("UI/coin");
                HUD_text.text = "+" + gameManager.rate;

                frogAudioSource.clip = frogAudioClips[0];
                frogAudioSource.Play();

            }
            else
            {
                anim.SetTrigger("mistake");
                gameManager.rate = 1;
                if(gameManager.difficultyLevel==0){
                    gameManager.health = gameManager.health - 1.0f;
                    HUD_text.text = "-" + 1;    
                }
                else{
                    gameManager.health = gameManager.health - (1.0f* gameManager.difficultyLevel);
                    HUD_text.text = "-" + gameManager.difficultyLevel;  
                }
                HUD_image.overrideSprite = Resources.Load<Sprite>("UI/time");
                HUD_image.transform.localScale = new Vector3(0.02f, 0.02f, 0);

                frogAudioSource.clip = frogAudioClips[1];
                frogAudioSource.Play();


            }

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
        if (this.gameObject.name.Equals("ball_" + (gameManager.startNumber + gameManager.seqNumber).ToString()))
            gameManager.canClick = false;


    }

    public void hideCircle()
    {
        anim.SetTrigger("spawn");
        this.transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;
        gameManager.canClick = true;
    }

}
}

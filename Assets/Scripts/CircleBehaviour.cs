using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Soomla.Store.IAP
{
public class CircleBehaviour : MonoBehaviour
{
    public Animator anim;
    GameManager GameManager.instance;
    public int ballNBR;

    Transform HUD;
    Text HUD_text;
    Image HUD_image;

    public AudioSource frogAudioSource;

    public AudioClip[] frogAudioClips;
    
    bool canClickOnCircle;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        HUD = gameObject.transform.FindChild("Canvas").FindChild("HUD");
        HUD_text = HUD.GetComponent<Text>();
        HUD_image = HUD.transform.FindChild("Image").GetComponent<Image>();
        frogAudioSource = this.GetComponent<AudioSource>();
        canClickOnCircle =true;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver == true)
        {
            anim.SetTrigger("wipe");
        }
        
        anim.speed=Random.RandomRange(0.5f,1.2f);


    }


    public void OnMouseDown()
    {
        if (GameManager.instance.canClick && canClickOnCircle==true)
        {
            anim.SetTrigger("hud_show");
            if (GameManager.instance.current == GameManager.instance.startNumber)
            {
                GameManager.instance.Hide();
                GameManager.instance.isHideClicked = true;
                //  GameManager.instance.coins += (GameManager.instance.current);

            }
            if (this.gameObject.name == "ball_" + GameManager.instance.current.ToString())
            {
                anim.SetTrigger("wipe");
                canClickOnCircle = false;
                GameManager.instance.current++;
                GameManager.instance.frogSmashed++;
                //  GameManager.instance.coins += 1 * GameManager.instance.rate;
                StoreInventory.GiveItem("coin_currency_id", 1 * GameManager.instance.rate);
                Destroy(this.gameObject, 1);
                GameManager.instance.health = GameManager.instance.health + 1.5f;
                HUD_image.overrideSprite = Resources.Load<Sprite>("UI/coin");
                HUD_text.text = "+" + (GameManager.instance.rate);
                
                frogAudioSource.clip = frogAudioClips[0];
                frogAudioSource.Play();
                GameManager.instance.rate++;
                if (GameManager.instance.rate > 3)
                {
                    GameManager.instance.rate = 3;
                }
                
                if (this.gameObject.name.Equals("ball_" + GameManager.instance.current.ToString()))
                { 
                    
                }

            }
            else
            {   
                anim.SetTrigger("mistake");
                GameManager.instance.ShakeCamera();
                GameManager.instance.rate = 1;
                if(GameManager.instance.difficultyLevel==0){
                    GameManager.instance.health = GameManager.instance.health - 1.0f;
                    HUD_text.text = "-" + 1;    
                }
                else{
                    GameManager.instance.health = GameManager.instance.health - (1.0f* GameManager.instance.difficultyLevel);
                    HUD_text.text = "-" + GameManager.instance.difficultyLevel;  
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
        Invoke("hideCircle", 4.0f);
        //  if (this.gameObject.name.Equals("ball_" + (GameManager.instance.startNumber + GameManager.instance.seqNumber).ToString()))
        //      GameManager.instance.canClick = false;


    }

    public void hideCircle()
    {
        anim.SetTrigger("spawn");
        this.transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;
        GameManager.instance.canClick = true;
    }
    
    void SetIsReadyTrue(){
        GameManager.instance.isReady=true;
    }

}
}

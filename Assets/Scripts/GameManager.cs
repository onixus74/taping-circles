using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Soomla;

namespace Soomla.Store.IAP
{
public class GameManager : MonoBehaviour
{

    public bool paused = false;
    public int startNumber = 0;
    public int seqNumber = 5;
    public int current = 0;
    public bool isReady = false;
    public float health = 10.0f;
    public float staretime = 2.0f;
    
    public GameObject ballsPrefab ; 

    // #### This is the level of difficulty  ####
    public int difficultyLevel = 0;

    // #### level difficulty Counter that increments difficulty level each 10 level Progress ####
    int levelProgress;

    // #### This is the current Level ####
    public int levelShow = 0;


    public bool canClick = true;
    public bool isGameOver = false;
    public int coins = 0;
    public float score = 0;

    public bool isHideClicked;
    GameObject wave;
    Animator wave_animation;


    public int rate = 1;

    void Start()
    {
        ballsPrefab = GameObject.FindGameObjectWithTag("balls");
        isHideClicked = false;
        current = startNumber;
        SpawnSpawner();
        SpawnCircleRange(startNumber, seqNumber, true);
        wave = GameObject.FindGameObjectWithTag("wave");
        wave_animation = wave.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        //  coins =  StoreInventory.GetItemBalance("coin_currency_id");
        //*** Wave touch feedback ***
        if (Input.GetMouseButtonDown(0))
        {
            wave.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8.0f));
            wave_animation.SetTrigger("wave");
        }

        health -= Time.deltaTime * 1.0f;
        if (current == seqNumber + startNumber)
        {
            current = -1;
            StartCoroutine("Wait", 1);

        }


        staretime -= Time.deltaTime;


        if (isHideClicked == false && staretime < 0)
        {
            Hide();
        }

        if (isReady)
        {
            staretime = 1 + seqNumber - Random.Range(levelProgress / seqNumber, 1);
            isHideClicked = false;
            //  StoreInventory.GiveItem("coin_currency_id",  Mathf.Abs(coins-StoreInventory.GetItemBalance(StoreInfo.Currencies[0].ItemId)));    
            //  StoreInventory.GiveItem("coin_currency_id",5);
            //How to give currency
            //  StoreInventory.GiveItem(StoreInfo.Currencies[0].ItemId,4000);
            
            startNumber = Random.Range(1, 90);
            if (difficultyLevel > 4)
            {
                seqNumber = Random.Range(difficultyLevel, difficultyLevel + 3);
            }
            else { seqNumber = Random.Range(4, 7); };

            SpawnCircleRange(startNumber, seqNumber, true);


        }

        checkGameOver();

    }

    void SpawnCircleRange(int start, int seqRange, bool isNumber)
    {

        levelProgress++;
        levelShow++;
        if (levelProgress == 10)
        {
            levelProgress = 1;
            difficultyLevel++;
        }

        //  canClick = false;
        current = start;
        List<int> array = new List<int>();
        GameObject ballTmp;
        int spawner;
        int counter = start;
        string test2 = "";
        if (isNumber)
        {
            for (int i = 0; i < seqRange; i++)
            {
                do
                {
                    spawner = Random.Range(1, 35);
                    if (array.Contains(spawner) == true)
                    {
                        spawner = Random.Range(1, 35);
                    }
                    else
                    {
                        array.Add(spawner);
                    }
                } while (array.Contains(spawner) == false);

            }

            foreach (int a in array)
            {
                test2 = test2 + " , " + a.ToString();
                ballTmp = Instantiate(Resources.Load("circle", typeof(GameObject)), GameObject.Find(a.ToString()).transform.position, Quaternion.identity) as GameObject;
                ballTmp.name = "ball_" + counter.ToString();
                ballTmp.transform.GetChild(0).GetComponentInChildren<Text>().text = counter.ToString();
                counter++;
                //  ballTmp.transform.parent = ballsPrefab.transform;
                ballTmp.transform.SetParent(ballsPrefab.transform);
            }

        }
        isReady = false;


    }

    void SpawnSpawner()
    {
        int count = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                count++;
                GameObject tmpObj = Instantiate(Resources.Load("Spawner", typeof(GameObject))) as GameObject;
                tmpObj.name = count.ToString();
                tmpObj.transform.position = new Vector3(j * 1.5f - 6.0f, i * -1.5f + 3, 9);
            }
        }
    }

    IEnumerator Wait(float s)
    {
        while (s > 0)
        {
            //     isReady = false;
            Debug.Log(s--);
            yield return new WaitForSeconds(1.0f);
        }
        if (s <= 0)
            isReady = true;

    }

    public void Hide()
    {   
        Transform[] allChildren = ballsPrefab.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) {
            if(child.name=="Canvas")
              child.transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;
        }
    }

    public void showSequentially()
    {
        //  canClick=false;
        if (coins > 300)
        {
            for (int i = startNumber; i < startNumber + seqNumber; i++)
            {
                GameObject tmp = GameObject.Find("ball_" + i.ToString());
                //  tmp.GetComponent<CircleBehaviour>().showCircle();
                tmp.GetComponent<CircleBehaviour>().Invoke("showCircle", Time.deltaTime * (i - startNumber) * 10);
            }
            coins -= 300;
        }
    }

    void NextFrame()
    {
        int i = startNumber;
        do
        {
            GameObject tmp = GameObject.Find("ball_" + i.ToString());
            tmp.GetComponent<CircleBehaviour>().showCircle();
            i++;

        } while (i < startNumber + seqNumber);
    }
    void checkGameOver()
    {


        if (health <= 0.05f)
        {
            isGameOver = true;
            health = 0;
        }
    }

    public void OnMouseDown()
    {
        wave.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }
    public void GiveCoin(int amount){
        StoreInventory.GiveItem("currency_coin", amount);
    }
}
}
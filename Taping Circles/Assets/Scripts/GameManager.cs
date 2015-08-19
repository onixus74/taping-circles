using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public bool paused = false;
    public int startNumber = 0;
    public int seqNumber = 5;
    public int current = 0;
    public bool isReady = false;
    public int level = 0;
    public float health = 0.8f;
    public float staretime = 2.0f;
    int levelProgress;
    public bool canClick = true;
    public int levelShow = 0;
    public bool isGameOver = false;
    public int coins = 0;
    public float score = 0;
    public GameObject circle = GameObject.Find("circle");

    public int rate = 1;

    void Start()
    {
        current = startNumber;
        SpawnSpawner();
        SpawnCircleRange(startNumber, seqNumber, true);
    }

    // Update is called once per frame
    void Update()
    {
        health -= Time.deltaTime * 0.03f;
        if (current == seqNumber + startNumber)

        {
            current = -1;
            StartCoroutine("Wait", 1);
        }
        if (isReady)
        {
            startNumber = Random.Range(1, 90);
            if (level > 3) { seqNumber = Random.Range(level - 3, level); } else { seqNumber = Random.Range(3, 5); };
            staretime = 1 + seqNumber - levelProgress / seqNumber;

            SpawnCircleRange(startNumber, seqNumber, true);
            Invoke("Hide", staretime);
        }

        checkGameOver();
    }

    void SpawnCircleRange(int start, int seqRange, bool isNumber)
    {
        health = health + 0.2f;
        score += health * 100;

        levelProgress++;
        levelShow++;
        if (levelProgress == 10)
        {
            levelProgress = 1;
            level++;
        }
        if (health >= 1)
        {
            health = 1.0f;
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
                Debug.Log("counter " + counter);
            }
            Debug.Log("array : " + test2);
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
                tmpObj.transform.position = new Vector3(j * 1.5f - 3.5f, i * -1.5f + 3, 9);
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
        Debug.Log("wait " + s.ToString());
    }

    public void Hide()
    {
        for (int i = startNumber; i < startNumber + seqNumber; i++)
        {
            GameObject tmp = GameObject.Find("ball_" + i.ToString());
            tmp.transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;
            Debug.Log("" + i + "is hidden");
        }
    }

    public void showSequentially()
    {
        //  canClick=false;
        if (coins > 1000)
        {
            for (int i = startNumber; i < startNumber + seqNumber; i++)
            {
                GameObject tmp = GameObject.Find("ball_" + i.ToString());
                //  tmp.GetComponent<CircleBehaviour>().showCircle();
                tmp.GetComponent<CircleBehaviour>().Invoke("showCircle", Time.deltaTime * (i - startNumber) * 10);
            }
            coins-=1000;
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








}

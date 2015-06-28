using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public bool paused = false;
    public GameObject ball;
    // public Transform proxyBall;

    public int startNumber = 0;
    public int seqNumber = 5;
    public int current = 0;
    public bool isReady = false;
    public bool canClick = false;

    public int level = 0;

    public float health = 0.8f;

    public float staretime = 2.0f;
    // Use this for initialization

    public GameObject circle = GameObject.Find("circle");
    void Start()
    {
        current = startNumber;
        SpawnSpawner();
        SpawnCircleRange(startNumber, seqNumber, true);
        canClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (current == seqNumber+startNumber)

        {
            current = -1;
            StartCoroutine("Wait", 1);
        }
        if (isReady)
        {
            startNumber = Random.Range(1, 90);
            seqNumber = Random.Range(3, level);
            staretime = seqNumber;
            SpawnCircleRange(startNumber, seqNumber, true);
            Invoke("Hide", staretime);
        }


    }

    void SpawnCircleRange(int start, int seqRange, bool isNumber)
    {
        health = health + 0.2f;
        level++;
        if (health >= 1)
        {
            health = 1.0f;
        }
        canClick = false;
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
                tmpObj.transform.position = new Vector3(j * 1.5f - 5.25f, i * -1.5f + 3, 9);
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

    void Hide()
    {
        for (int i = startNumber; i < startNumber + seqNumber; i++)
        {
            GameObject tmp = GameObject.Find("ball_" + i.ToString());
            tmp.transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;
        }
        canClick = true;
    }


}

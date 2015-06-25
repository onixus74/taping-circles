using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public bool paused = false;
    public GameObject ball;
    // public Transform proxyBall;

    public int fixedNumber = 5;
    public int ballNumber = 5;
    public int current = 0;

    public int test = 0;


    // Use this for initialization

    public GameObject circle = GameObject.Find("circle");
    void Start()
    {
        SpawnSpawner();
        SpawnCircleRange(1, 10, true);

    }

    // Update is called once per frame
    void Update()
    {

        current = ballNumber - (fixedNumber - 1);
    }

    void SpawnCircleRange(int start, int seqRange, bool isNumber)
    {
        List<int> array = new List<int>();
        GameObject ballTmp;
        int spawner;
        if (isNumber)
        {
            for (int i = start; i < start + seqRange; i++)
            {
                do
                {
                    spawner = Random.Range(1, 35);
                    if (array.Contains(spawner) == false)
                    {
                        array.Add(spawner);
                    }
                    else
                    {
                        spawner = Random.Range(1, 35);
                    }
                } while (array.Contains(spawner) == false);

                ballTmp = Instantiate(Resources.Load("circle",typeof(GameObject)), GameObject.Find(spawner.ToString()).transform.position, Quaternion.identity) as GameObject;
                ballTmp.name = "ball_" + i.ToString();
                //  ballTmp.>GetComponent<Text>().text = i.ToString();
                ballTmp.transform.GetChild(0).GetComponentInChildren<Text>().text=i.ToString();

            }
        }
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


}

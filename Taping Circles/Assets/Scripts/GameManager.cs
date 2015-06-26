using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public bool paused = false;
    public GameObject ball;
    // public Transform proxyBall;

    public int startNumber = 0;
    public int seqNumber = 5;
    public int current = 0;

    // Use this for initialization

    public GameObject circle = GameObject.Find("circle");
    void Start()
    {
        current = startNumber;
        SpawnSpawner();
        SpawnCircleRange(startNumber, seqNumber, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(current==seqNumber){
             SpawnCircleRange(startNumber, seqNumber, true);
        }

    }

    void SpawnCircleRange(int start, int seqRange, bool isNumber)
    {   
        current=start;
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
                    //Debug
                    //

                } while (array.Contains(spawner) == false);

            }
            // for (int i = start; i < start + seqRange; i++)
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

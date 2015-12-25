using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public GameObject BugPrefab;
 
    public void SpawnBug(){
      Invoke("SpawnNow",1f);
    }
    void SpawnNow(){
         GameObject go = Instantiate(BugPrefab);
         GooglyEye.LookAt = go.transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour

{
    public int isAlive = 2;
    private TimeScript timeScript;
    public GameObject Deer;


    void Start() {
        timeScript = GetComponent<TimeScript>();
    }
    void Update()
    {
        if (isAlive <= 1)
        {
            SpawnAnother();
        }
    }
    void SpawnAnother()
    {
        timeScript.deerCount++;
        float x = Random.Range(-947, 593);
        float z = Random.Range(-412, 1030);
        Vector3 DeerSpawnPosition = new Vector3(x, 0, z);
        isAlive += 1;
        GameObject Deerclone = Instantiate(Deer, DeerSpawnPosition, Deer.transform.rotation);
        Debug.Log("Another Deer Has Spawned!");
    }
}

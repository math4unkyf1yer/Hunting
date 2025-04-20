using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour

{
    public int isAlive = 2;
    public int spawnBack = 2;
    private TimeScript timeScript;
    public GameObject Enemy;
    public bool isAdeer;
    public bool isABird;
    public float xRangeMin = -700;
    public float xRangeMax = 270;
    public float zRangeMin = -212;
    public float zRangeMax = 690;
    public float yRange;

    void Start() {
        timeScript = GetComponent<TimeScript>();
    }
    void Update()
    {
       
        if (isAlive < spawnBack)
        {
            SpawnAnother();
        }
    }
    void SpawnAnother()
    {
        if(isAdeer == true)
        {
            timeScript.enemyCount++;
            float x = Random.Range(xRangeMin, xRangeMax);
            float z = Random.Range(zRangeMin, zRangeMax);
            Vector3 DeerSpawnPosition = new Vector3(x, 0, z);
            isAlive += 1;
            GameObject EnemyClone = Instantiate(Enemy, DeerSpawnPosition, Enemy.transform.rotation);
            Debug.Log("Another Deer Has Spawned!");
        }
        if(isABird == true)
        {
            timeScript.enemyCount++;
            float x = Random.Range(xRangeMin, xRangeMax);
            float z = Random.Range(zRangeMin, zRangeMax);
            Vector3 DeerSpawnPosition = new Vector3(x, yRange, z);
            isAlive += 1;
            GameObject EnemyClone = Instantiate(Enemy, DeerSpawnPosition, Enemy.transform.rotation);
            Debug.Log("Another Deer Has Spawned!");
        }
    }
}

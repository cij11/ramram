using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawnables;
    public int spawnDelay = 5;
    public bool allowDuplicates = false;

    GameObject lastSpawned;

    float spawnTimer;

	// Use this for initialization
	void Start () {
        spawnTimer = 0f;
		
	}
	
	// Update is called once per frame
	void Update () {

        UpdateTimers();
		
	}

    void UpdateTimers()
    {
        if (lastSpawned == null)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnDelay)
            {
                SpawnObject();
                spawnTimer = 0.0f;
            }
        }
    }

    void SpawnObject()
    {
        int objectToSpawnIndex = Random.Range(0, spawnables.Length);

        GameObject objectToSpawn = spawnables[objectToSpawnIndex];

        lastSpawned = Instantiate(objectToSpawn, this.transform.position, Quaternion.identity);
    }
}

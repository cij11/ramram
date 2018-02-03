using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawnables;
    public int spawnDelay = 5;
    public bool allowDuplicates = false;

    float spawnRadius = 5.0f;

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
        //Pick a random object
        int objectToSpawnIndex = Random.Range(0, spawnables.Length);
        GameObject objectToSpawn = spawnables[objectToSpawnIndex];
    
        //Pick a random location within spawnRadius of the spawner.
        Vector3 randomVector = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.0f, Random.Range(-spawnRadius, spawnRadius));
        lastSpawned = Instantiate(objectToSpawn, this.transform.position + randomVector, Quaternion.identity);
    }
}

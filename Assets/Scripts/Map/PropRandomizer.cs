using UnityEngine;
using System.Collections.Generic;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> PropSpawnPoints;
    public List<GameObject> PropPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnProps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject spawnPoint in PropSpawnPoints)
        {
            int randomIndex = Random.Range(0, PropPrefabs.Count);
            GameObject propPrefab = PropPrefabs[randomIndex];
            GameObject prop = Instantiate(propPrefab, spawnPoint.transform.position, Quaternion.identity);
            prop.transform.parent = spawnPoint.transform; // Set the spawn point as the parent of the prop
        }
    }
}

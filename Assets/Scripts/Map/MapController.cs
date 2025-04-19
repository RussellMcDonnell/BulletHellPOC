using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> TerrainChuncks;
    public GameObject Player;
    public float CheckerRadius;
    public LayerMask terrainMask;
    public GameObject CurrentTerrainChunck;

    [Header("Optimization")]
    public List<GameObject> SpawnedChuncks;
    private GameObject _latestChunck;
    public float MaxOptimizationDistance = 10f; // Distance to check for optimization
    private float _optimizationDistance;
    private float _optimizationCooldown;
    public float OptimizationCooldownDuration;

    private List<string> _directions = new List<string> { "Right", "Left", "Up", "Down", "Right Up", "Right Down", "Left Up", "Left Down" };

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimzer();
    }

    void ChunkChecker()
    {
        if (!CurrentTerrainChunck) return; // Check if CurrentTerrainChunck is null
        //string directionName = GetDirectionName(moveDirection); // Get the direction name based on the movement direction

        // Check all directions for spawning new chunks
        foreach (string directionName in _directions)
        {
            Vector3 spawnPosition = CurrentTerrainChunck.transform.Find(directionName).position; // Get the spawn position based on the direction name
            if (!Physics2D.OverlapCircle(spawnPosition, CheckerRadius, terrainMask))
            {
                SpawnChunk(spawnPosition); // Spawn a new chunk at the spawn position
            }
        }

    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int randomIndex = Random.Range(0, TerrainChuncks.Count);
        _latestChunck = Instantiate(TerrainChuncks[randomIndex], spawnPosition, Quaternion.identity);
        SpawnedChuncks.Add(_latestChunck);
    }

    void ChunkOptimzer()
    {
        // Optimize how often the optimization check is performed to reduce performance impact
        if (_optimizationCooldown > 0f)
        {
            _optimizationCooldown -= Time.deltaTime; // Decrease the cooldown timer
            return; // Skip optimization if cooldown is active
        }
        else
        {
            _optimizationCooldown = OptimizationCooldownDuration; // Reset the cooldown timer
        }

        foreach (GameObject chunk in SpawnedChuncks)
        {
            _optimizationDistance = Vector3.Distance(Player.transform.position, chunk.transform.position);
            if (_optimizationDistance > MaxOptimizationDistance)
            {
                chunk.SetActive(false); // Deactivate the chunk
            }
            else
            {
                chunk.SetActive(true); // Activate the chunk if within distance
            }
        }
    }
}

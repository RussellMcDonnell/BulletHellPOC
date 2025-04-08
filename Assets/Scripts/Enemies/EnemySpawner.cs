using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for spawning enemies in waves.
/// TODO this could be improved by using a pool of enemies instead of instantiating them every time.
/// TODO this could be improved/changed the way waves are spawned to better fit the game design.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    private Transform _playerTransform;

    [System.Serializable]
    public class Wave
    {
        public string WaveName;
        public List<EnemyGroup> EnemyGroups; // List of enemy groups to spawn in this wave
        public int WaveQuota; // The total number of enemies to spawn in this wave
        public float SpawnInterval; // The interval at which to spawn enemies
        public float SpawnCount; // The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string EnemyName;
        public GameObject EnemyPrefab; // The prefab of the enemy to spawn
        public int EnemyCount; // The number of enemies to spawn in this group
        public int SpawnedCount; // The number of enemies already spawned in this group
    }

    public List<Wave> Waves; // List of all of the waves in the game
    public int CurrentWaveIndex; // The index of the current wave being spawned [0-based]

    [Header("Spawner Attributes")]
    private float _spawnTimer; // Timer used to determine when to spawn the next enemy
    private int _enemiesAlive;
    public int MaxEnemiesAllowed; // The maximum number of enemies allowed to be spawned on the map at once
    public bool MaxEnemiesReached = false; // Flag to check if the maximum number of enemies has been reached
    public float WaveInterval; // The interval between waves

    [Header("Spawn Positions")]
    public List<Transform> RelativeSpawnPositions; // List of spawn positions for the enemies

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerTransform = FindAnyObjectByType<PlayerStats>().transform; // Find the player object in the scene
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWaveIndex < Waves.Count && Waves[CurrentWaveIndex].SpawnCount == 0) // Check if the current wave index is valid
        {
            StartCoroutine(BeginNextWave()); // Start the coroutine to begin the next wave
        }

        _spawnTimer += Time.deltaTime; // Increment the spawn timer by the time since the last frame

        if (_spawnTimer >= Waves[CurrentWaveIndex].SpawnInterval) // Check if the spawn timer has reached the spawn interval
        {
            _spawnTimer = 0f; // Reset the spawn timer
            SpawnEnemies(); // Spawn enemies for the current wave
        }
    }

    private IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(WaveInterval); // Wait for the specified wave interval before starting the next wave

        
        if (CurrentWaveIndex < Waves.Count - 1) // Check if all waves have been completed
        {
            CurrentWaveIndex++; // Move to the next wave
            CalculateWaveQuota(); // Calculate the quota for the next wave
        }
    }

    private void CalculateWaveQuota()
    {
        // Calculate the total number of enemies to spawn in the current wave
        int currentTotalWaveQuota = 0;
        foreach (var enemyGroup in Waves[CurrentWaveIndex].EnemyGroups)
        {
            currentTotalWaveQuota += enemyGroup.EnemyCount;
        }

        Debug.Log("Current Wave Quota: " + currentTotalWaveQuota);
        Waves[CurrentWaveIndex].WaveQuota = currentTotalWaveQuota;
    }

    private void SpawnEnemies()
    {
        // Reset the flag for maximum enemies reached
        if(_enemiesAlive < MaxEnemiesAllowed)
        {
            MaxEnemiesReached = false; // Reset the flag if the number of enemies is below the maximum allowed
        }

        // Check if the maximum number of enemies has been reached or the current wave has reached its quota
        if (MaxEnemiesReached || Waves[CurrentWaveIndex].SpawnCount >= Waves[CurrentWaveIndex].WaveQuota)
        {
            return;
        }

        // Loop through each enemy group in the current wave
        foreach (var enemyGroup in Waves[CurrentWaveIndex].EnemyGroups)
        {
            // Check if the group has spawned all its enemies
            if (enemyGroup.SpawnedCount >= enemyGroup.EnemyCount)
                continue; // Skip to the next group if all enemies are spawned

            // Limit the number of enemies that can be spawned at once
            if (_enemiesAlive >= MaxEnemiesAllowed)
            {
                MaxEnemiesReached = true; // Set the flag if the maximum number of enemies is reached
                return; // Exit if the maximum number of enemies is reached
            }
            
            // Spawn the enemy prefab at a random spawn position relative to the player
            Instantiate(enemyGroup.EnemyPrefab, _playerTransform.position + RelativeSpawnPositions[Random.Range(0, RelativeSpawnPositions.Count)].position, Quaternion.identity);

            // Increment the spawn count for this group and the total spawn count for the wave
            enemyGroup.SpawnedCount++;
            Waves[CurrentWaveIndex].SpawnCount++;
            _enemiesAlive++; // Increment the total number of enemies alive
        }
    }

    // Method to be called when an enemy dies
    public void OnEnemyDeath()
    {
        _enemiesAlive--; // Decrement the total number of enemies alive when one dies
    }
}

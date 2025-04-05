using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float WaveInterval; // The interval between waves

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

        Debug.LogWarning("Current Wave Quota: " + currentTotalWaveQuota);
        Waves[CurrentWaveIndex].WaveQuota = currentTotalWaveQuota;
    }

    private void SpawnEnemies()
    {
        // Check if the current wave has reached its quota
        if (Waves[CurrentWaveIndex].SpawnCount >= Waves[CurrentWaveIndex].WaveQuota)
        {
            Debug.Log("Wave " + Waves[CurrentWaveIndex].WaveName + " completed!");
            return; // Exit if the wave is complete
        }

        // Loop through each enemy group in the current wave
        foreach (var enemyGroup in Waves[CurrentWaveIndex].EnemyGroups)
        {
            // Check if the group has spawned all its enemies
            if (enemyGroup.SpawnedCount >= enemyGroup.EnemyCount)
                continue; // Skip to the next group if all enemies are spawned

            Vector2 spawnPosition = new Vector2(_playerTransform.position.x + Random.Range(-10f, 10f), _playerTransform.position.y + Random.Range(-10f, 10f));

            // Spawn the enemy prefab at the spawner's position
            GameObject spawnedEnemy = Instantiate(enemyGroup.EnemyPrefab, spawnPosition, Quaternion.identity);

            // Increment the spawn count for this group and the total spawn count for the wave
            enemyGroup.SpawnedCount++;
            Waves[CurrentWaveIndex].SpawnCount++;
        }
    }
}

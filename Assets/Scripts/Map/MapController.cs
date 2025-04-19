using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> TerrainChuncks;
    public GameObject Player;
    public float CheckerRadius;
    public LayerMask terrainMask;
    public GameObject CurrentTerrainChunck;
    private Vector3 _playerLstPosition;

    [Header("Optimization")]
    public List<GameObject> SpawnedChuncks;
    private GameObject _latestChunck;
    public float MaxOptimizationDistance = 10f; // Distance to check for optimization
    private float _optimizationDistance;
    private float _optimizationCooldown;
    public float OptimizationCooldownDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerLstPosition = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChunckChecker();
        ChunkOptimzer();
    }

    void ChunckChecker()
    {
        if (!CurrentTerrainChunck) return; // Check if CurrentTerrainChunck is null

        Vector3 moveDirection = Player.transform.position - _playerLstPosition; // Calculate the movement direction of the player
        _playerLstPosition = Player.transform.position; // Update the last position of the player

        string directionName = GetDirectionName(moveDirection); // Get the direction name based on the movement direction


        Vector3 spawnPosition = CurrentTerrainChunck.transform.Find(directionName).position; // Get the spawn position based on the direction name
        if(!Physics2D.OverlapCircle(spawnPosition, CheckerRadius, terrainMask)) // Check if the area in the specified direction is empty
        {            
            SpawnChunck(spawnPosition); // Spawn a new chunk at the spawn position
        }

    }

    private string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        bool isPositivePrimary = (isHorizontal ? direction.x : direction.y) > 0;
        bool hasSecondary = Mathf.Abs(isHorizontal ? direction.y : direction.x) > 0.5f;
        bool isPositiveSecondary = (isHorizontal ? direction.y : direction.x) > 0;

        if (isHorizontal)
        {
            if (hasSecondary)
                return isPositivePrimary
                    ? (isPositiveSecondary ? "Right Up" : "Right Down")
                    : (isPositiveSecondary ? "Left Up" : "Left Down");
            else
                return isPositivePrimary ? "Right" : "Left";
        }
        else
        {
            if (hasSecondary)
                return isPositivePrimary
                    ? (isPositiveSecondary ? "Up Right" : "Up Left")
                    : (isPositiveSecondary ? "Down Right" : "Down Left");
            else
                return isPositivePrimary ? "Up" : "Down";
        }
    }


    void SpawnChunck(Vector3 spawnPosition)
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

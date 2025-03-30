using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Vector3 _noTerrainPosition;
    private PlayerMovement _playerMovement;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
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

        if (_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y == 0) //right
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Right").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Right").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y == 0) //left
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Left").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Left").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x == 0 && _playerMovement.moveDirection.y > 0) //up
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Up").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Up").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x == 0 && _playerMovement.moveDirection.y < 0) //down
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Down").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Down").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y > 0) //right up
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Right Up").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Right Up").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y < 0) //right down
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Right Down").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Right Down").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y < 0) //left down
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Left Down").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Left Down").position;
                SpawnChunck();
            }
        }
        else if (_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y > 0) //left up
        {
            if (!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Left Up").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Left Up").position;
                SpawnChunck();
            }
        }
    }

    void SpawnChunck()
    {
        int randomIndex = Random.Range(0, TerrainChuncks.Count);
        _latestChunck = Instantiate(TerrainChuncks[randomIndex], _noTerrainPosition, Quaternion.identity);
        SpawnedChuncks.Add(_latestChunck);
    }

    void ChunkOptimzer()
    {
        // Optimize how often the optimization check is performed to reduce performance impact
        if (_optimizationCooldown > 0f)
        {
            _optimizationCooldown -= Time.deltaTime; // Decrease the cooldown timer
            return; // Skip optimization if cooldown is active
        } else
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

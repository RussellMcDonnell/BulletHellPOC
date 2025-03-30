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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunckChecker();
    }

    void ChunckChecker()
    {
        if(!CurrentTerrainChunck) return; // Check if CurrentTerrainChunck is null

        if(_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y == 0) //right
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Right").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Right").position;
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y == 0) //left
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Left").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Left").position;
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x == 0 && _playerMovement.moveDirection.y > 0) //up
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Up").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Up").position;
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x == 0 && _playerMovement.moveDirection.y < 0) //down
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Down").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Down").position;
                SpawnChunck();
            }
        }
        else if(_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y > 0) //right up
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Right Up").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Right Up").position;
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y < 0) //right down
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Right Down").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Right Down").position;
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y < 0) //left down
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Left Down").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Left Down").position;
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y > 0) //left up
        {
            if(!Physics2D.OverlapCircle(CurrentTerrainChunck.transform.Find("Left Up").position, CheckerRadius, terrainMask))
            {
                _noTerrainPosition = CurrentTerrainChunck.transform.Find("Left Up").position;
                SpawnChunck();
            }
        }
    }

    void SpawnChunck()
    {
        int randomIndex = Random.Range(0, TerrainChuncks.Count);
        Instantiate(TerrainChuncks[randomIndex], _noTerrainPosition, Quaternion.identity);
    }
}

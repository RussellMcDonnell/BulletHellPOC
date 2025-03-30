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
        if(_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y == 0) //right
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(20, 0, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(20, 0, 0);
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y == 0) //left
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(-20, 0, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(-20, 0, 0);
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x == 0 && _playerMovement.moveDirection.y > 0) //up
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(0, 20, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(0, 20, 0);
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x == 0 && _playerMovement.moveDirection.y < 0) //down
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(0, -20, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(0, -20, 0);
                SpawnChunck();
            }
        }
        else if(_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y > 0) //up right
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(20, 20, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(20, 20, 0);
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x > 0 && _playerMovement.moveDirection.y < 0) //down right
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(20, -20, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(20, -20, 0);
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y < 0) //down left
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(-20, -20, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(-20, -20, 0);
                SpawnChunck();
            }
        } else if(_playerMovement.moveDirection.x < 0 && _playerMovement.moveDirection.y > 0) //up left
        {
            if(!Physics2D.OverlapCircle(Player.transform.position + new Vector3(-20, 20, 0), CheckerRadius, terrainMask))
            {
                _noTerrainPosition = Player.transform.position + new Vector3(-20, 20, 0);
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

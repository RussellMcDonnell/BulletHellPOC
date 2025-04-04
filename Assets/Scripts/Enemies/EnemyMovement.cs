using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats _enemyStats;
    private Transform _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyStats = GetComponent<EnemyStats>();
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _enemyStats.CurrentMovementSpeed * Time.deltaTime); // Constantly move towards the player
    }
}

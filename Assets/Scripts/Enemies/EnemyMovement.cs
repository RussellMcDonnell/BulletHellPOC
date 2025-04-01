using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject EnemyData;
    private Transform _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, EnemyData.MovementSpeed * Time.deltaTime); // Constantly move towards the player
    }
}

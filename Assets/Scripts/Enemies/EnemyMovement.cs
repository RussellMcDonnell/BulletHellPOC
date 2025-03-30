using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform _player;
    public float MovementSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, MovementSpeed * Time.deltaTime); // Constantly move towards the player
    }
}

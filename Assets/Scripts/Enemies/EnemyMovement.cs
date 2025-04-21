using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats _enemyStats;
    private Transform _player;

    private Vector2 _knockbackVelocity;
    private float _knockbackDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyStats = GetComponent<EnemyStats>();
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_knockbackDuration > 0)
        {
            transform.position += (Vector3)_knockbackVelocity * Time.deltaTime; // Apply knockback effect
            _knockbackDuration -= Time.deltaTime; // Decrease the knockback duration
        }
        else
        {
            // constantly move towards the player
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _enemyStats.CurrentMovementSpeed * Time.deltaTime); // Constantly move towards the player
        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (_knockbackDuration > 0) return; // If already in knockback, do nothing

        _knockbackVelocity = velocity;
        _knockbackDuration = duration;
    }
}

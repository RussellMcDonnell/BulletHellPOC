using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerStats _playerStats;
    private CircleCollider2D _playerCollector;

    public float PullSpeed;

    private void Start()
    {
        _playerStats = FindAnyObjectByType<PlayerStats>();
        _playerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // Update player collector to match magnet radius
        _playerCollector.radius = _playerStats.CurrentMagnet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the ICollectible interface
        if (collision.TryGetComponent(out ICollectible collectible))
        {
            // Pulling animation
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Vector2 forceDireation = ((Vector2)transform.position - rb.position).normalized; // Direction to the player
            rb.AddForce(forceDireation.normalized * PullSpeed * Time.deltaTime, ForceMode2D.Impulse); // Pull the collectible towards the player

            collectible.Collect();
        }
    }
}

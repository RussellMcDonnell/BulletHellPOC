using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerCollector : MonoBehaviour
{
    private PlayerStats _playerStats;
    private CircleCollider2D _detector;

    public float PullSpeed;

    private void Start()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
    }

    public void SetDetectorRadius(float radius)
    {
        if(!_detector)
            _detector = GetComponent<CircleCollider2D>();
        _detector.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is a pickup
        if (collision.TryGetComponent(out Pickup collectible))
        {
            collectible.Collect(_playerStats, PullSpeed);
        }
    }
}
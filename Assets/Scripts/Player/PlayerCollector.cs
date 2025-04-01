using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the ICollectible interface
        if (collision.TryGetComponent(out ICollectible collectible))
        {
            collectible.Collect();
        }
    }
}

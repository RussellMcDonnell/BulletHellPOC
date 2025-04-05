using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with the player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the gem after collection
        }
    }
}

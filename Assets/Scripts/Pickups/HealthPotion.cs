using UnityEngine;

public class HealthPotion : MonoBehaviour, ICollectible
{
    public int HealthToRestore; // Amount of health restored by the potion

    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.RestoreHealth(HealthToRestore);
        Destroy(gameObject); // Destroy the potion after collection
    }
}

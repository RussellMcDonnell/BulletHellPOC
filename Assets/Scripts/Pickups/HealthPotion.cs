using UnityEngine;

public class HealthPotion : Pickup, ICollectible
{
    public int HealthToRestore; // Amount of health restored by the potion

    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.RestoreHealth(HealthToRestore);
    }
}

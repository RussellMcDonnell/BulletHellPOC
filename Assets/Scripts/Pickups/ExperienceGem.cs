using UnityEngine;

public class ExperienceGem : Pickup, ICollectible
{
    public int ExperienceGranted;
    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.IncreaseExperience(ExperienceGranted);
    }
}

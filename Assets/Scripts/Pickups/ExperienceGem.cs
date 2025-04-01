using UnityEngine;

public class ExperienceGem : MonoBehaviour, ICollectible
{
    public int ExperienceGranted;
    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.IncreaseExperience(ExperienceGranted);
        Destroy(gameObject);
    }
}

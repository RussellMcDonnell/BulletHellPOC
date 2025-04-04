using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Current stats
    [HideInInspector]
    public float CurrentHealth;
    [HideInInspector]
    public float CurrentRecovery;
    [HideInInspector]
    public float CurrentMovementSpeed;
    [HideInInspector]
    public float CurrentMight;
    [HideInInspector]
    public float CurrentProjectileSpeed;

    // Experience and level of the Player
    public int Experience = 0;
    public int Level = 1;
    public int ExperienceCap;

    // Class for defining a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int StartLevel;
        public int EndLevel;
        public int ExperienceCapIncrease;
    }

    public List<LevelRange> LevelRanges;

    // I-Frames
    [Header("I-Frames")]
    public float InvincibilityDuration; // Duration of invincibility after taking damage
    private float _invincibilityTimer; // Timer for invincibility
    private bool _isInvincible; // Flag to check if the player is currently invincible

    public CharacterScriptableObject CharacterData;

    private void Awake()
    {
        // Initialize the current stats to the values from the CharacterData scriptable object
        CurrentHealth = CharacterData.MaxHealth;
        CurrentRecovery = CharacterData.Recovery;
        CurrentMovementSpeed = CharacterData.MovementSpeed;
        CurrentMight = CharacterData.Might;
        CurrentProjectileSpeed = CharacterData.ProjectileSpeed;
    }

    private void Start()
    {
        // Initialize the experience cap based on the level ranges defined in the inspector
        ExperienceCap = LevelRanges[0].ExperienceCapIncrease;
        for (int i = 1; i < LevelRanges.Count; i++)
        {
            if (Level >= LevelRanges[i].StartLevel && Level <= LevelRanges[i].EndLevel)
            {
                ExperienceCap += LevelRanges[i].ExperienceCapIncrease;
            }
        }
    }

    private void Update()
    {
        // Update the invincibility timer if the player is invincible
        if (_isInvincible)
        {
            _invincibilityTimer -= Time.deltaTime; // Decrease the timer by the time since last frame
            if (_invincibilityTimer <= 0)
            {
                _isInvincible = false; // Reset the invincibility flag when the timer reaches zero
            }
        }
    }

    public void IncreaseExperience(int amount)
    {
        // Increase the player's experience by the specified amount
        Experience += amount;

        // Check if the player has reached the experience cap for leveling up
        LevelUpChecker();
    }

    private void LevelUpChecker()
    {
        // Check if the player has reached the experience cap for leveling up
        if (Experience >= ExperienceCap)
        {
            Level++;
            Experience -= ExperienceCap; // Reset experience to 0 after leveling up

            int experienceCapIncrease = 0;
            foreach (LevelRange range in LevelRanges)
            {
                if (Level >= range.StartLevel && Level <= range.EndLevel)
                {
                    experienceCapIncrease = range.ExperienceCapIncrease;
                    break;
                }
            }
            ExperienceCap += experienceCapIncrease; // Increase the experience cap for the next level
        }
    }
    
    public void TakeDamage(float damage)
    {
         // If the player is invincible, ignore damage
        if (_isInvincible) return;

        _invincibilityTimer = InvincibilityDuration; // Reset the invincibility timer
        _isInvincible = true; // Set the player to invincible

        // Reduce current health by the damage taken
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die(); // Call the die method if health is zero or less
        }
    }

    public void Die()
    {
        // Handle player death (e.g., play animation, destroy object, etc.)
        Debug.Log("Player has died!"); // Placeholder for player death handling
        Destroy(gameObject); // Destroy the player game object
    }

    public void RestoreHealth(float healAmount)
    {
        if (CurrentHealth == CharacterData.MaxHealth) return; // If health is already at max, do nothing

        // Restore health to the player
        CurrentHealth += healAmount;
        if (CurrentHealth > CharacterData.MaxHealth)
        {
            CurrentHealth = CharacterData.MaxHealth; // Ensure health does not exceed max health
        }
    }
}

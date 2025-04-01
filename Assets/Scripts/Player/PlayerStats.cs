using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Current stats
    private float _currentHealth;
    private float _currentRecovery;
    private float _currentMovementSpeed;
    private float _currentMight;
    private float _currentProjectileSpeed;

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

    public CharacterScriptableObject CharacterData;

    private void Awake()
    {
        // Initialize the current stats to the values from the CharacterData scriptable object
        _currentHealth = CharacterData.MaxHealth;
        _currentRecovery = CharacterData.Recovery;
        _currentMovementSpeed = CharacterData.MovementSpeed;
        _currentMight = CharacterData.Might;
        _currentProjectileSpeed = CharacterData.ProjectileSpeed;
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

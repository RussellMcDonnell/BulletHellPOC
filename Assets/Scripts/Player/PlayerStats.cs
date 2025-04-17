using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // Current stats
    [HideInInspector]
    public float CurrentHealth;
    [HideInInspector]
    public float CurrentMaxHealth;
    [HideInInspector]
    public float CurrentRecovery;
    [HideInInspector]
    public float CurrentMovementSpeed;
    [HideInInspector]
    public float CurrentMight;
    [HideInInspector]
    public float CurrentProjectileSpeed;
    [HideInInspector]
    public float CurrentMagnet;

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

    private CharacterScriptableObject _characterData;
    public PlayerCollector PlayerCollector;

    private InventoryManager _inventoryManager;
    private int _weaponIndex;
    private int _passiveItemIndex;

    [Header("UI")]
    public Image HealthBar;
    public Image ExperienceBar;
    public TextMeshProUGUI LevelText;

    private void Awake()
    {
        _characterData = CharacterSelector.GetCharacterData(); // Get the character data from the CharacterSelector singleton
        CharacterSelector.Instance.DestroySingleton();

        _inventoryManager = GetComponent<InventoryManager>(); // Get the InventoryManager component from the player

        // Initialize the current stats to the values from the CharacterData scriptable object
        CurrentHealth = CurrentMaxHealth = _characterData.MaxHealth; // Set current health and max health
        CurrentRecovery = _characterData.Recovery;
        CurrentMovementSpeed = _characterData.MovementSpeed;
        CurrentMight = _characterData.Might;
        CurrentProjectileSpeed = _characterData.ProjectileSpeed;
        CurrentMagnet = _characterData.Magnet;

        // Initialize the player collector component
        PlayerCollector = GetComponentInChildren<PlayerCollector>();
        PlayerCollector.SetDetectorRadius(CurrentMagnet); // Set the detector radius based on the character data

        //Spawn the staring weapon
        SpawnWeapon(_characterData.StartingWeapon); // Spawn the starting weapon for the player
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
        
        GameManager.Instance.AssignChosenCharacterUI(_characterData); // Assign the character UI in the GameManager

        UpdateHealthBar();
        UpdateExperienceBar();
        UpdateLevelText();
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
        
        Recover(); // Call the recover method to restore health over time
    }

    public void IncreaseExperience(int amount)
    {
        // Increase the player's experience by the specified amount
        Experience += amount;

        // Check if the player has reached the experience cap for leveling up
        LevelUpChecker();

        UpdateExperienceBar(); // Update the experience bar UI
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

            UpdateExperienceBar(); // Update the experience bar UI
            GameManager.Instance.StartLevelingUp(); // Notify the GameManager to start the leveling up process
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

        UpdateHealthBar();
    }

    public void Die()
    {
        if(!GameManager.Instance.CurrentState.Equals(GameManager.GameState.GameOver))
        {
            GameManager.Instance.AssignChosenWeaponsAndPassivesItemsUI(_inventoryManager.WeaponIcons, _inventoryManager.PassiveItemIcons); // Assign the UI for the chosen weapons and passive items
            GameManager.Instance.AssignLevelReachedUI(Level);
            GameManager.Instance.GameOver();
        }
    }

    public void RestoreHealth(float healAmount)
    {
        if (CurrentHealth == CurrentMaxHealth) return; // If health is already at max, do nothing

        // Restore health to the player
        CurrentHealth += healAmount;
        if (CurrentHealth > CurrentMaxHealth)
        {
            CurrentHealth = CurrentMaxHealth; // Ensure health does not exceed max health
        }

        UpdateHealthBar();
    }

    private void Recover()
    {
        if (CurrentHealth > CurrentMaxHealth) return; // If health is already at max, do nothing

        // Restore health over time based on the recovery rate
        CurrentHealth += CurrentRecovery * Time.deltaTime;
        if (CurrentHealth > CurrentMaxHealth)
        {
            CurrentHealth = CurrentMaxHealth; // Ensure health does not exceed max health
        }

        UpdateHealthBar();
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (_weaponIndex >= _inventoryManager.WeaponSlots.Count -1)
        {
            Debug.LogError("Trying to add a weapon when the Inventory slots are already full!"); // Log an error if there are no available weapon slots
            return; // Exit the method if there are no available slots
        }


        // Spawn a weapon and add it to the list of spawned weapons
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // Set the parent of the spawned weapon to the player
        _inventoryManager.AddWeapon(_weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); // Add the weapon to the inventory slot
        _weaponIndex++; // Increment the weapon index for the next weapon
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (_passiveItemIndex >= _inventoryManager.PassiveItemSlots.Count - 1)
        {
            Debug.LogError("Trying to add a passive item when the Inventory slots are already full!"); // Log an error if there are no available passive item slots
            return; // Exit the method if there are no available slots
        }

        // Spawn a passive item and add it to the list of spawned passive items
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); // Set the parent of the spawned passive item to the player
        _inventoryManager.AddPassiveItem(_passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); // Add the passive item to the inventory slot
        _passiveItemIndex++; // Increment the passive item index for the next item
    }

    private void UpdateHealthBar()
    {
        // Update the health bar UI based on the current health
        HealthBar.fillAmount = CurrentHealth / CurrentMaxHealth;
    }

    private void UpdateExperienceBar()
    {
        // Update the experience bar UI based on the current experience
        ExperienceBar.fillAmount = (float)Experience / ExperienceCap;
    }

    private void UpdateLevelText()
    {
        // Update the level text UI
        LevelText.text = "LV " + Level.ToString();
    }
}

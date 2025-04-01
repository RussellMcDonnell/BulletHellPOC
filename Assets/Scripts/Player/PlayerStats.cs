using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Current stats
    private float _currentHealth;
    private float _currentRecovery;
    private float _currentMovementSpeed;
    private float _currentMight;
    private float _currentProjectileSpeed;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

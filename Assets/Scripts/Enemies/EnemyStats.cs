using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject EnemyData;

    // current stats
    private float _currentHealth;
    private float _currentDamage;
    private float _currentMovementSpeed;

    private void Awake()
    {
        // Initialize current stats based on the scriptable object values
        _currentHealth = EnemyData.MaxHealth;
        _currentDamage = EnemyData.Damage;
        _currentMovementSpeed = EnemyData.MovementSpeed;

    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage; // Reduce current health by the damage taken
        if (_currentHealth <= 0)
        {
            Die(); // Call the die method if health is zero or less
        }
    }

    public void Die()
    {
        // Handle enemy death (e.g., play animation, destroy object, etc.)
        Destroy(gameObject); // Destroy the enemy game object
    }
}

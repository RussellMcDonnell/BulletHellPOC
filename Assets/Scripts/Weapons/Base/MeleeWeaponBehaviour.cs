using UnityEngine;

/// <summary>
/// Base script of all melee behaviours [To be placed on the weapon prefab of a melee weapon]
/// </summary>
public abstract class MeleeWeaponBehaviour : MonoBehaviour
{
    // Current stats
    protected float _currentDamage;
    protected float _currentSpeed;
    protected float _currentCooldownDuration;
    protected float _currentPierce;

    public WeaponScriptableObject WeaponData;
    public float DestroyAfterSeconds;

    private void Awake()
    {
        // Initialize the current stats to the values from the WeaponData scriptable object
        _currentDamage = WeaponData.Damage;
        _currentSpeed = WeaponData.Speed;
        _currentCooldownDuration = WeaponData.CooldownDuration;
        _currentPierce = WeaponData.Pierce;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, DestroyAfterSeconds); // Destroy the object after a certain amount of time
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object collided with has the tag "Enemy"
        if (collider.CompareTag("Enemy"))
        {
            // Get the EnemyStats component from the collided object
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            if (enemy == null)
                return; // If the enemy stats component is not found, exit the method

            // Call the TakeDamage method on the enemy stats with the current damage
            enemy.TakeDamage(_currentDamage);
        }
    }
}

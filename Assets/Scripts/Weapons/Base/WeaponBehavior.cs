using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    // Current stats
    protected float _currentDamage;
    protected float _currentSpeed;
    protected float _currentCooldownDuration;

    public WeaponScriptableObject WeaponData;
    public float DestroyAfterSeconds;

    protected virtual void Awake()
    {
        // Initialize the current stats to the values from the WeaponData scriptable object
        _currentDamage = WeaponData.Damage;
        _currentSpeed = WeaponData.Speed;
        _currentCooldownDuration = WeaponData.CooldownDuration;
    }

     public float GetCurrentDamage()
    {
        // Get the current damage and multiply it by the player's might
        return _currentDamage *= FindAnyObjectByType<PlayerStats>().CurrentMight;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, DestroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        //Refference the script from the collider and deal damage using TakeDamage()
        if (collider.CompareTag("Enemy"))
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            if (enemy == null)
                return; // If the enemy is null, exit the method

            enemy.TakeDamage(GetCurrentDamage(), transform.position); // Make suer to use GetCurrentDamage() instead of WeaponData.Damage to use the current stats in case any damage multiplier is applied
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                breakableProp.TakeDamage(GetCurrentDamage()); // Make sure to use GetCurrentDamage() instead of WeaponData.Damage to use the current stats in case any damage multiplier is applied
            }
        }
    }
}

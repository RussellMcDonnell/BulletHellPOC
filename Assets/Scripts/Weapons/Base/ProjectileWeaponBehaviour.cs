using UnityEngine;

/// <summary>
/// Base script of all the projectile behaviours [To be placed on a prefab of a weapon that is a projectile]
/// </summary>
public abstract class ProjectileWeaponBehaviour : MonoBehaviour
{
    // Current stats
    protected float _currentDamage;
    protected float _currentSpeed;
    protected float _currentCooldownDuration;
    protected float _currentPierce;

    protected Vector3 _direction;
    public WeaponScriptableObject WeaponData;
    public float DestroyAfterSeconds;


    void Awake()
    {
        // Initialize the current stats to the values from the WeaponData scriptable object
        _currentDamage = WeaponData.Damage;
        _currentSpeed = WeaponData.Speed;
        _currentCooldownDuration = WeaponData.CooldownDuration;
        _currentPierce = WeaponData.Pierce;
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

    public void DirectionChecker(Vector3 direction)
    {
        _direction = direction;

        float directionX = _direction.x;
        float directionY = _direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.localRotation.eulerAngles;

        // set the rotation of the object based on the direction
        if (directionX < 0 && directionY == 0) //left
        {
            rotation.z = 135f;
        }
        else if (directionX > 0 && directionY == 0) //right
        {
            rotation.z = -45f;
        }
        else if (directionX == 0 && directionY < 0) //down
        {
            rotation.z = -135f;
        }
        else if (directionX == 0 && directionY > 0) //up
        {
            rotation.z = 45f;
        }
        else if (directionX < 0 && directionY < 0) //down left
        {
            rotation.z = -180f;
        }
        else if (directionX > 0 && directionY < 0) //down right
        {
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY > 0) //up left
        {
            rotation.z = 90f;
        }
        else if (directionX > 0 && directionY > 0) //up right
        {
            rotation.z = 0f;
        }

        transform.localScale = scale; // Set the scale of the object to the new scale
        transform.rotation = Quaternion.Euler(rotation); // Set the rotation of the object to the new rotation
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        //Refference the script from the collider and deal damage using TakeDamage()
        if (collider.CompareTag("Enemy"))
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            if (enemy == null)
                return; // If the enemy is null, exit the method

            enemy.TakeDamage(GetCurrentDamage()); // Make suer to use GetCurrentDamage() instead of WeaponData.Damage to use the current stats in case any damage multiplier is applied
            ReducePierce(); // Reduce the pierce value after hitting an enemy
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                breakableProp.TakeDamage(GetCurrentDamage()); // Make sure to use GetCurrentDamage() instead of WeaponData.Damage to use the current stats in case any damage multiplier is applied
                ReducePierce(); // Reduce the pierce value after hitting a prop
            }
        }
    }

    private void ReducePierce()
    {
        _currentPierce -= 1; // Reduce the pierce value by 1
        if (_currentPierce <= 0)
        {
            Destroy(gameObject); // Destroy the object if pierce is 0 or less
        }
    }
}

using UnityEngine;

/// <summary>
/// Base script of all the projectile behaviours [To be placed on a prefab of a weapon that is a projectile]
/// </summary>
public abstract class ProjectileWeaponBehaviour : WeaponBehavior
{
    protected Vector3 _direction;
    protected float _currentPierce;

    protected override void Awake()
    {
        // Initialize the current stats to the values from the WeaponData scriptable object
        _currentPierce = WeaponData.Pierce;
        base.Awake(); // Call the base Awake method to ensure any additional initialization is done
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

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        //Refference the script from the collider and deal damage using TakeDamage()
        if (collider.CompareTag("Enemy"))
        {
            ReducePierce(); // Reduce the pierce value after hitting an enemy
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                ReducePierce(); // Reduce the pierce value after hitting a prop
            }
        }

        base.OnTriggerEnter2D(collider); // Call the base method to handle the collision
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

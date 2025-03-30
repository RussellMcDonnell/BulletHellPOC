using UnityEngine;

/// <summary>
/// Base script of all the projectile behaviours [To be placed on a prefab of a weapon that is a projectile]
/// </summary>
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 _direction;
    public float DestroyAfterSeconds;


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
        if(directionX < 0 && directionY == 0) //left
        {
            rotation.z = 135f;
        }
        else if(directionX > 0 && directionY == 0) //right
        {
            rotation.z = -45f;
        }
        else if(directionX == 0 && directionY < 0) //down
        {
            rotation.z = -135f;
        }
        else if(directionX == 0 && directionY > 0) //up
        {
            rotation.z = 45f;
        }
        else if(directionX < 0 && directionY < 0) //down left
        {
            rotation.z = -180f;
        }
        else if(directionX > 0 && directionY < 0) //down right
        {
            rotation.z = -90f;
        }
        else if(directionX < 0 && directionY > 0) //up left
        {
            rotation.z = 90f;
        }
        else if(directionX > 0 && directionY > 0) //up right
        {
            rotation.z = 0f;
        }

        transform.localScale = scale; // Set the scale of the object to the new scale
        transform.rotation = Quaternion.Euler(rotation); // Set the rotation of the object to the new rotation
    }
}

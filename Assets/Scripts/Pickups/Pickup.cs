using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float LifeSpan = 3.5f; // Time in seconds before the pickup is destroyed
    protected PlayerStats _target; // If the pickup has a target, fly towards the target
    protected float _speed; // Speed at which the pickup travels towards the target

    [Header("Bonuses")]
    public int ExperienceGranted; // Amount of experience granted by the pickup
    public int HealthToRestore; // Amount of health restored by the pickup

    protected virtual void Update()
    {
        // If the pickup has a target, move towards it
        if (_target != null)
        {
            // Move towards the target and check the distance between the pickup and the target
            Vector2 distance = _target.transform.position - transform.position;
            if (distance.sqrMagnitude > _speed * _speed * Time.deltaTime)
            {
                transform.position += (Vector3)distance.normalized * _speed * Time.deltaTime; // Move towards the target
            }
            else
            {
                Destroy(gameObject); // Destroy the pickup if it is close enough to the target
            }
            transform.position += (Vector3)distance * _speed * Time.deltaTime; // Move towards the target
            
            // Destroy the pickup after its lifespan has expired
            LifeSpan -= Time.deltaTime;
            if (LifeSpan <= 0f)
            {
                Destroy(gameObject);
            }
        }

    }

    public virtual bool Collect(PlayerStats target, float speed, float lifeSpan = 0f)
    {
        if (!_target)
        {
            _target = target; // Set the target to the player stats
            _speed = speed; // Set the speed of the pickup
            if (lifeSpan > 0)
                LifeSpan = lifeSpan; // Set the lifespan of the pickup
            Destroy(gameObject, Mathf.Max(0.01f, LifeSpan));
            return true; // Return true if the pickup was collected successfully
        }
        else
        {
            return false; // Return false if the pickup was already collected
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with the player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the gem after collection
        }
    }

    protected virtual void OnDestroy()
    {
        if (!_target)
            return; // If the target is null, exit the method
        if (ExperienceGranted != 0)
        {
            _target.IncreaseExperience(ExperienceGranted); // Grant experience to the player
        }

        if (HealthToRestore != 0)
        {
            _target.RestoreHealth(HealthToRestore); // Restore health to the player
        }
    }
}

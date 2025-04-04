using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    private List<GameObject> _markedEnemies = new List<GameObject>(); // List to keep track of marked enemies

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); // Call the base class Start method to handle destruction
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy") && !_markedEnemies.Contains(collider.gameObject))
        {
            // Get the EnemyStats component from the collided object
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            if (enemy == null)
                return; // If the enemy component is not found, exit the method

            // Call the TakeDamage method on the enemy with the current damage
            enemy.TakeDamage(_currentDamage);

            _markedEnemies.Add(collider.gameObject); // Add the enemy to the list of marked enemies so it doesn't take another instance of damage from this Garlic
        }
        else if (collider.CompareTag("Prop") && !_markedEnemies.Contains(collider.gameObject))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                breakableProp.TakeDamage(_currentDamage); // Make sure to use _currentDamage instead of WeaponData.Damage to use the current stats in case any damage multiplier is applied
                
                _markedEnemies.Add(collider.gameObject); // Add the enemy to the list of marked enemies so it doesn't take another instance of damage from this Garlic
            }
        }
    }
}

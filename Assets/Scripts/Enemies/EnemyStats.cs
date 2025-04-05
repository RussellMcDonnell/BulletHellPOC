using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject EnemyData;
    private DropRateManager _dropRateManager;

    // current stats
    [HideInInspector]
    public float CurrentHealth;
    [HideInInspector]
    public float CurrentDamage;
    [HideInInspector]
    public float CurrentMovementSpeed;

    public float DespawnDistance = 20f;
    private Transform _playerTransform;
    

    private void Start()
    {
        // Find the player transform in the scene
        _playerTransform = FindAnyObjectByType<PlayerStats>().transform;
    }

    private void Update()
    {
        // Check if the enemy is outside the despawn distance from the player
        if (Vector2.Distance(_playerTransform.position, transform.position) > DespawnDistance)
        {
            ReturnEnemy();
        }
    }

    private void Awake()
    {
        // Initialize current stats based on the scriptable object values
        CurrentHealth = EnemyData.MaxHealth;
        CurrentDamage = EnemyData.Damage;
        CurrentMovementSpeed = EnemyData.MovementSpeed;
        _dropRateManager = GetComponent<DropRateManager>();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage; // Reduce current health by the damage taken
        if (CurrentHealth <= 0)
        {
            Die(); // Call the die method if health is zero or less
        }
    }

    /// <summary>
    /// Method to handle enemy death (e.g., play animation, destroy object, etc.)
    /// </summary>
    public void Die()
    {
        if(_dropRateManager != null)
        {
            _dropRateManager.HandleDrop(); // Call the drop loot method if the drop rate manager is present
        }

        Destroy(gameObject); // Destroy the enemy game object
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the object collided with has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerStats component from the collided object
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player == null)
                return; // If the player stats component is not found, exit the method

            // Call the TakeDamage method on the player stats with the current damage
            player.TakeDamage(CurrentDamage);
        }
    }

    private void OnDestroy()
    {
        // TODO this could be converted to an event system
        EnemySpawner enemySpawner = FindAnyObjectByType<EnemySpawner>(); // Find the enemy spawner in the scene
        enemySpawner.OnEnemyDeath(); // Notify the enemy spawner of the enemy's death
    }

    private void ReturnEnemy()
    {
       EnemySpawner enemySpawner = FindAnyObjectByType<EnemySpawner>(); // Find the enemy spawner in the scene
       
       // Return the enemy to a random spawn position relative to the player
       transform.position = _playerTransform.position + enemySpawner.RelativeSpawnPositions[Random.Range(0, enemySpawner.RelativeSpawnPositions.Count)].position;
    }
}

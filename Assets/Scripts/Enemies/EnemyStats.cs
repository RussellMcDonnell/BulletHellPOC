using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SpriteRenderer))]
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

    [Header("Damage Feedback")]
    public Color DamageColor = new Color(1, 0, 0, 1); // Red color for damage feedback
    public float DamageFlashDuration = 0.2f;
    public float DeathFadeDuration = 0.6f;
    private Color originalColor;
    private SpriteRenderer _spriteRenderer;
    private EnemyMovement _enemyMovement;
    

    private void Start()
    {
        // Find the player transform in the scene
        _playerTransform = FindAnyObjectByType<PlayerStats>().transform;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = _spriteRenderer.color; // Store the original color of the sprite renderer
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private IEnumerator DamageFlash()
    {
        _spriteRenderer.color = DamageColor; // Set the sprite color to the damage color
        yield return new WaitForSeconds(DamageFlashDuration); // Wait for the damage flash duration
        _spriteRenderer.color = originalColor; // Reset the sprite color to the original color
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

    public void TakeDamage(float damage, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        CurrentHealth -= damage; // Reduce current health by the damage taken
        StartCoroutine(DamageFlash()); // Start the damage flash coroutine

        // Create the text popup for damage feedback
        if (damage > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(damage).ToString(), transform);
        }

        if(knockbackForce > 0)
        {
            // Get the direction of the knockback based on the source position
            Vector2 knockbackDirection = (Vector2)transform.position - sourcePosition;
            _enemyMovement.Knockback(knockbackDirection.normalized * knockbackForce, knockbackDuration); // Apply knockback to the enemy
        }

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

        //Try to get the BoxCollider2D component attached to the enemy
        var collider = GetComponent<BoxCollider2D>();
        if(collider)
        {
            // Disable the collider to prevent further collisions
            collider.enabled = false;
        }

        // Start the kill fade coroutine to handle the death animation and destruction
        StartCoroutine(KillFade()); // Start the kill fade coroutine
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
        enemySpawner?.OnEnemyDeath(); // Notify the enemy spawner of the enemy's death
    }

    private void ReturnEnemy()
    {
       EnemySpawner enemySpawner = FindAnyObjectByType<EnemySpawner>(); // Find the enemy spawner in the scene
       
       // Return the enemy to a random spawn position relative to the player
       transform.position = _playerTransform.position + enemySpawner.RelativeSpawnPositions[UnityEngine.Random.Range(0, enemySpawner.RelativeSpawnPositions.Count)].position;
    }

    private IEnumerator KillFade()
    {
        // Wait for a single frame
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float t = 0, originalAlpha = _spriteRenderer.color.a; // Store the original alpha value of the sprite renderer

        while (t < DeathFadeDuration) {
            yield return waitForEndOfFrame; // Wait for the end of the frame
            t += Time.deltaTime; // Increment the time variable

            // Set the color for this frame
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, Mathf.Lerp(originalAlpha, 0, t / DeathFadeDuration)); // Lerp the alpha value to create a fade-out effect
        }

        Destroy(gameObject); // Destroy the enemy game object after the fade-out effect
    }
}

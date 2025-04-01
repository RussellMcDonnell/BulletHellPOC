using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject WeaponData;
    private float _currentCooldown;

    protected PlayerMovement _playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        // Find the PlayerMovement component in the scene
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        // Initialize the weapon cooldown to the cooldown duration
        _currentCooldown = WeaponData.CooldownDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _currentCooldown -= Time.deltaTime;

        // Once the cooldown becomes less than or equal to 0, we can attack again
        if (_currentCooldown <= 0f)
        {
            Attack();
        }

    }

    protected virtual void Attack()
    {
        // Re-initialize the weapon cooldown to the cooldown duration
        _currentCooldown = WeaponData.CooldownDuration;
    }
}

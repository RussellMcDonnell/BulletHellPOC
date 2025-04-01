using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    // Base stats for enemies
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _damage;

    public float MovementSpeed {get => _movementSpeed; private set => _movementSpeed = value; }
    public float MaxHealth {get => _maxHealth; private set => _maxHealth = value; }
    public float Damage {get => _damage; private set => _damage = value; }
}

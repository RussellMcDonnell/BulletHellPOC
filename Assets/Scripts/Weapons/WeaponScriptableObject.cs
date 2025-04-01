using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "Scriptable Objects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    private GameObject _weaponPrefab;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _cooldownDuration;
    [SerializeField]
    private int _pierce;

    public GameObject WeaponPrefab { get => _weaponPrefab; private set => _weaponPrefab = value; }
    public float Damage { get => _damage; private set => _damage = value; }
    public float Speed { get => _speed; private set => _speed = value; }
    public float CooldownDuration { get => _cooldownDuration; private set => _cooldownDuration = value; }
    public int Pierce { get => _pierce; private set => _pierce = value; }
}

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
    [SerializeField]
    private int _level; // Not ment to be modified in the game, only in the editor
    [SerializeField]
    private GameObject _nextLevelPrefab; // Prefab for the next level of the weapon e.g. what the weapon will become when it levels up
    [SerializeField] // Not ment to be modified in the game, only in the editor
    private Sprite _icon; // Icon for the weapon, used in the inventory UI
    [SerializeField]
    private string _weaponName;
    [SerializeField]
    private string _description;
    [SerializeField]
    private int evolvedUpgradeToRemove; // The index of the weapon that will be removed when this weapon is evolved. This is used to remove the weapon from the inventory when it is evolved.

    public GameObject WeaponPrefab { get => _weaponPrefab; private set => _weaponPrefab = value; }
    public float Damage { get => _damage; private set => _damage = value; }
    public float Speed { get => _speed; private set => _speed = value; }
    public float CooldownDuration { get => _cooldownDuration; private set => _cooldownDuration = value; }
    public int Pierce { get => _pierce; private set => _pierce = value; }
    public int Level { get => _level; private set => _level = value; }
    public GameObject NextLevelPrefab { get => _nextLevelPrefab; private set => _nextLevelPrefab = value; }
    public Sprite Icon { get => _icon; private set => _icon = value; }
    public string WeaponName { get => _weaponName; private set => _weaponName = value; }
    public string Description { get => _description; private set => _description = value; }
    public int EvolvedUpgradeToRemove { get => evolvedUpgradeToRemove; private set => evolvedUpgradeToRemove = value; }
}

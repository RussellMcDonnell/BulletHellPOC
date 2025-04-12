using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Scriptable Objects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    private string _characterName;
    [SerializeField]
    private Sprite _characterSprite;
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private GameObject _startingWeapon;
    [SerializeField]
    private float _recovery;
    [SerializeField]
    private float _might;
    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private float _magnet;


    public string CharacterName { get => _characterName; private set => _characterName = value; }
    public Sprite CharacterSprite { get => _characterSprite; private set => _characterSprite = value; }
    public float MovementSpeed { get => _movementSpeed; private set => _movementSpeed = value; }
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public GameObject StartingWeapon { get => _startingWeapon; private set => _startingWeapon = value; }
    public float Recovery { get => _recovery; private set => _recovery = value; }
    public float Might { get => _might; private set => _might = value; }
    public float ProjectileSpeed { get => _projectileSpeed; private set => _projectileSpeed = value; }
    public float Magnet { get => _magnet; private set => _magnet = value; }
}

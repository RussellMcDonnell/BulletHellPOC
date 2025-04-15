using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObjectScript", menuName = "Scriptable Objects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private float _multiplier;
    [SerializeField]
    private int _level; // Not ment to be modified in the game, only in the editor

    // Prefab for the next level of the weapon e.g. what the weapon will become when it levels up
    [SerializeField]
    private GameObject _nextLevelPrefab;

    // Icon for the weapon, used in the inventory UI
    [SerializeField] // Not ment to be modified in the game, only in the editor
    private Sprite _icon;
    [SerializeField]
    private string _passiveName;
    [SerializeField]
    private string _description;

    public float Multiplier { get => _multiplier; private set => _multiplier = value; }
    public int Level { get => _level; private set => _level = value; }
    public GameObject NextLevelPrefab { get => _nextLevelPrefab; private set => _nextLevelPrefab = value; }
    public Sprite Icon { get => _icon; private set => _icon = value; }
    public string PassiveName { get => _passiveName; private set => _passiveName = value; }
    public string Description { get => _description; private set => _description = value; }
}

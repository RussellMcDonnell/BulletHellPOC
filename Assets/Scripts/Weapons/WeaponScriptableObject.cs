using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "Scriptable Objects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject weaponPrefab;
    public float Damaage;
    public float Speed;
    public float CooldownDuration;    
    public int Pierce;
}

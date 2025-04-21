using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEvolutionBlueprint", menuName = "Scriptable Objects/WeaponEvolutionBlueprint")]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    public WeaponScriptableObject BaseWeaponData;
    public PassiveItemScriptableObject CatalystPassiveItemData;
    public WeaponScriptableObject EvolvedWeaponData;
    public GameObject EvolvedWeapon;    
}

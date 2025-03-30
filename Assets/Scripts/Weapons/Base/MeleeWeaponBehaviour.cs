using UnityEngine;

/// <summary>
/// Base script of all melee behaviours [To be placed on the weapon prefab of a melee weapon]
/// </summary>
public abstract class MeleeWeaponBehaviour : MonoBehaviour
{
    public float DestroyAfterSeconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, DestroyAfterSeconds); // Destroy the object after a certain amount of time
    }
}

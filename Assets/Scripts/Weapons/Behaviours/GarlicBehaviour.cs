using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); // Call the base class Start method to handle destruction
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class GarlicController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack(); // Call the base class Attack method to handle attack logic

        // Spawn the garlic prefab at the player's position
        GameObject spawnedGarlic = Instantiate(WeaponData.weaponPrefab, transform.position, Quaternion.identity);
        spawnedGarlic.transform.parent = transform; // So it spawns below the player
    }
}

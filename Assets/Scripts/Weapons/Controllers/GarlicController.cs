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
        GameObject spawnedGarlic = Instantiate(WeaponData.WeaponPrefab, transform.position, Quaternion.identity);
        spawnedGarlic.transform.position = transform.position; // So it spawns below the player
        spawnedGarlic.transform.parent = transform;
    }
}

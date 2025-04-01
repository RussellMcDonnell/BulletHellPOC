using UnityEngine;

public class KnifeController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(WeaponData.weaponPrefab);
        spawnedKnife.transform.position = transform.position; // Assign the position of the knife to the same as the object which is parented to (the player)
        spawnedKnife.GetComponent<KnifeBehaviour>().DirectionChecker(_playerMovement.LastMoveVector); // Get the direction of the knife
    }
}

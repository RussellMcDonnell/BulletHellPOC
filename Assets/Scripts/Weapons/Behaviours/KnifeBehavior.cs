using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * _currentSpeed * Time.deltaTime; // Set the movement of the Knife
    }
}

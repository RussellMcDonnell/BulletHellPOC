using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    private KnifeController _knifeController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _knifeController = FindAnyObjectByType<KnifeController>();        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * _knifeController.Speed * Time.deltaTime; // Set the movement of the Knife
    }
}

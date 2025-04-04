using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    private DropRateManager _dropRateManager;

    public float health;

    private void Awake()
    {
        _dropRateManager = GetComponent<DropRateManager>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        if (_dropRateManager != null)
        {
            _dropRateManager.HandleDrop(); // Call the drop loot method if the drop rate manager is present
        }

        Destroy(gameObject);
    }
}

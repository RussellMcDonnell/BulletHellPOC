using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Assuming you have a method to add items to the inventory
            OpenTreasureChest();
            Destroy(gameObject); // Destroy the chest after collecting it
        }
        
    }

    public void OpenTreasureChest()
    {
        
    }
}

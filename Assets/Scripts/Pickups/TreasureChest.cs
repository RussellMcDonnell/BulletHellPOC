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
        var possibleEvolutions = inventoryManager.GetPossibleEvolutions();

        if(possibleEvolutions.Count <= 0)
        {
            Debug.Log("No possible evolutions available.");
            return;
        }

        // If there is more than one weapon to evolve, randomly select one and evolve it
        WeaponEvolutionBlueprint weaponToEvolve = possibleEvolutions[Random.Range(0, possibleEvolutions.Count)];
        inventoryManager.EvolveWeapon(weaponToEvolve);
    }
}

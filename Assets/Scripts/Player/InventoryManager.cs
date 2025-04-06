using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> WeaponSlots = new List<WeaponController>(6);
    public List<PassiveItem> PassiveItemSlots = new List<PassiveItem>(6);

    public int[] WeaponLevels = new int[6];
    public int[] PassiveItemLevels = new int[6];

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        WeaponSlots[slotIndex] = weapon;
        WeaponLevels[slotIndex] = weapon.WeaponData.Level;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        PassiveItemSlots[slotIndex] = passiveItem;
        PassiveItemLevels[slotIndex] = passiveItem.PassiveItemData.Level;
    }

    public void UpgradeWeapon(int slotIndex)
    {
        if (WeaponSlots.Count > slotIndex)
        {
            WeaponController weapon = WeaponSlots[slotIndex];
            if (weapon != null)
            {
                if(!weapon.WeaponData.NextLevelPrefab)
                {
                    Debug.LogError("Next level prefab is not set for the weapon: " + weapon.name);
                    return;
                }

                GameObject upgradedWeaponPrefab = Instantiate(weapon.WeaponData.NextLevelPrefab, transform.position, Quaternion.identity);
                upgradedWeaponPrefab.transform.SetParent(transform); // Set the parent of the spawned weapon to the player
                WeaponController upgradedWeapon = upgradedWeaponPrefab.GetComponent<WeaponController>();
                
                // Add the weapon to the inventory slot
                AddWeapon(slotIndex, upgradedWeapon);
                Destroy(weapon.gameObject);
                WeaponLevels[slotIndex] = upgradedWeapon.WeaponData.Level; // Update the weapon level
            }
        }
    }

    public void UpgradePassiveItem(int slotIndex)
    {
        if (PassiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = PassiveItemSlots[slotIndex];
            if (passiveItem != null)
            {
                if(!passiveItem.PassiveItemData.NextLevelPrefab)
                {
                    Debug.LogError("Next level prefab is not set for the passive item: " + passiveItem.name);
                    return;
                }

                GameObject upgradedPassiveItemPrefab = Instantiate(passiveItem.PassiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
                upgradedPassiveItemPrefab.transform.SetParent(transform); // Set the parent of the spawned passive item to the player
                PassiveItem upgradedPassiveItem = upgradedPassiveItemPrefab.GetComponent<PassiveItem>();
                 
                // Add the passive item to the inventory slot
                AddPassiveItem(slotIndex, upgradedPassiveItem);
                Destroy(passiveItem.gameObject);
                PassiveItemLevels[slotIndex] = upgradedPassiveItem.PassiveItemData.Level; // Update the passive item level
            }
        }        
    }
}

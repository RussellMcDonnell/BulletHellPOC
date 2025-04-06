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
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        PassiveItemSlots[slotIndex] = passiveItem;
    }

    public void UpgradeWeapon(int slotIndex)
    {
        if (WeaponLevels[slotIndex] < 3)
        {
            WeaponLevels[slotIndex]++;
            // TODO Logic to upgrade the weapon
        }
    }

    public void UpgradePassiveItem(int slotIndex)
    {
        if (PassiveItemLevels[slotIndex] < 3)
        {
            PassiveItemLevels[slotIndex]++;
            // TODO Logic to upgrade the passive item
        }
    }
}

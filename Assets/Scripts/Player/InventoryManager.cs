using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> WeaponSlots = new List<WeaponController>(6);
    public List<PassiveItem> PassiveItemSlots = new List<PassiveItem>(6);

    public List<Image> WeaponIcons = new List<Image>(6);
    public List<Image> PassiveItemIcons = new List<Image>(6);

    public int[] WeaponLevels = new int[6];
    public int[] PassiveItemLevels = new int[6];


    [Serializable]
    public class WeaponUpgrade
    {
        public int WeaponUpgradeIndex;
        public GameObject InitialWeapon;
        public WeaponScriptableObject WeaponData;
    }

    [Serializable]
    public class PassiveItemUpgrade
    {
        public int PassiveItemUpgradeIndex;
        public GameObject InitialPassiveItem;
        public PassiveItemScriptableObject PassiveItemData;
    }

    [Serializable]
    public class UpgradeUI
    {
        public TextMeshProUGUI UpgradeNameDisplay;
        public TextMeshProUGUI UpgradeDescriptionDisplay;
        public Image UpgradeIcon;
        public Button UpgradeButton;
    }

    public List<WeaponUpgrade> WeaponUpgradeOptions = new List<WeaponUpgrade>(); // List of upgrade options for weapons
    public List<PassiveItemUpgrade> PassiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of upgrade options for passive items
    public List<UpgradeUI> UpgradeUIOptions = new List<UpgradeUI>(); // List of UI for Upgrade options present in the scene

    private PlayerStats _player;

    private void Start()
    {
        _player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        WeaponSlots[slotIndex] = weapon;
        WeaponLevels[slotIndex] = weapon.WeaponData.Level;
        WeaponIcons[slotIndex].sprite = weapon.WeaponData.Icon;
        WeaponIcons[slotIndex].enabled = true; // Enable the icon

        if (GameManager.Instance != null && GameManager.Instance.ChoosingUpgrade)
        {
            GameManager.Instance.EndLevelingUp(); // End the leveling up phase
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        PassiveItemSlots[slotIndex] = passiveItem;
        PassiveItemLevels[slotIndex] = passiveItem.PassiveItemData.Level;
        PassiveItemIcons[slotIndex].sprite = passiveItem.PassiveItemData.Icon;
        PassiveItemIcons[slotIndex].enabled = true; // Enable the icon
        if (GameManager.Instance != null && GameManager.Instance.ChoosingUpgrade)
        {
            GameManager.Instance.EndLevelingUp(); // End the leveling up phase
        }
    }

    public void UpgradeWeapon(int slotIndex, int upgradeIndex)
    {
        if (WeaponSlots.Count > slotIndex)
        {
            WeaponController weapon = WeaponSlots[slotIndex];
            if (weapon != null)
            {
                if (!weapon.WeaponData.NextLevelPrefab)
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
            
                WeaponUpgradeOptions[upgradeIndex].WeaponData = upgradedWeapon.GetComponent<WeaponController>().WeaponData;
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.ChoosingUpgrade)
        {
            GameManager.Instance.EndLevelingUp(); // End the leveling up phase
        }
    }

    public void UpgradePassiveItem(int slotIndex, int upgradeIndex)
    {
        if (PassiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = PassiveItemSlots[slotIndex];
            if (passiveItem != null)
            {
                if (!passiveItem.PassiveItemData.NextLevelPrefab)
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

                PassiveItemUpgradeOptions[upgradeIndex].PassiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().PassiveItemData;
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.ChoosingUpgrade)
        {
            GameManager.Instance.EndLevelingUp(); // End the leveling up phase
        }
    }

    private void ApplyUpgradeOptions()
    {
        foreach (var upgradeOption in UpgradeUIOptions)
        {
            int upgradeType = UnityEngine.Random.Range(1, 3); // Randomly select the type of upgrade (1 for weapon, 2 for passive item)

            if (upgradeType == 1) // Weapon upgrade
            {
                // Select a random weapon upgrade option
                int randomIndex = UnityEngine.Random.Range(0, WeaponUpgradeOptions.Count);
                WeaponUpgrade selectedWeaponUpgrade = WeaponUpgradeOptions[randomIndex];

                if (selectedWeaponUpgrade.InitialWeapon == null)
                {
                    Debug.LogError("Initial weapon is not set for the weapon upgrade: " + selectedWeaponUpgrade.WeaponData.WeaponName);
                    continue;
                }

                bool newWeapon = true;
                // Check if the weapon is already in the inventory
                for (int i = 0; i < WeaponSlots.Count; i++)
                {
                    if (WeaponSlots[i] != null && WeaponSlots[i].WeaponData == selectedWeaponUpgrade.WeaponData)
                    {
                        newWeapon = false;
                        if (!newWeapon)
                        {
                            upgradeOption.UpgradeButton.onClick.AddListener(() => UpgradeWeapon(i, selectedWeaponUpgrade.WeaponUpgradeIndex));
                            upgradeOption.UpgradeNameDisplay.text = selectedWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponController>().WeaponData.WeaponName;
                            upgradeOption.UpgradeDescriptionDisplay.text = selectedWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponController>().WeaponData.Description;
                        }
                        break;
                    }
                }
                if (newWeapon)
                {
                    //Spawn a new weapon
                    upgradeOption.UpgradeButton.onClick.AddListener(() => _player.SpawnWeapon(selectedWeaponUpgrade.InitialWeapon));
                    upgradeOption.UpgradeNameDisplay.text = selectedWeaponUpgrade.WeaponData.WeaponName;
                    upgradeOption.UpgradeDescriptionDisplay.text = selectedWeaponUpgrade.WeaponData.Description;
                }

                upgradeOption.UpgradeIcon.sprite = selectedWeaponUpgrade.WeaponData.Icon;
            }
            else // Passive item upgrade
            {
                // Select a random passive item upgrade option
                int randomIndex = UnityEngine.Random.Range(0, PassiveItemUpgradeOptions.Count);
                PassiveItemUpgrade selectedPassiveItemUpgrade = PassiveItemUpgradeOptions[randomIndex];

                if (selectedPassiveItemUpgrade.InitialPassiveItem == null)
                {
                    Debug.LogError("Initial passive item is not set for the passive item upgrade: " + selectedPassiveItemUpgrade.PassiveItemData.PassiveName);
                    continue;
                }

                bool newPassiveItem = true;
                // Check if the passive item is already in the inventory
                for (int i = 0; i < PassiveItemSlots.Count; i++)
                {
                    if (PassiveItemSlots[i] != null && PassiveItemSlots[i].PassiveItemData == selectedPassiveItemUpgrade.PassiveItemData)
                    {
                        newPassiveItem = false;
                        if (!newPassiveItem)
                        {
                            upgradeOption.UpgradeButton.onClick.AddListener(() => UpgradePassiveItem(i, selectedPassiveItemUpgrade.PassiveItemUpgradeIndex));
                            upgradeOption.UpgradeNameDisplay.text = selectedPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().PassiveItemData.PassiveName;
                            upgradeOption.UpgradeDescriptionDisplay.text = selectedPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().PassiveItemData.Description;
                        }
                        break;
                    }
                }
                if (newPassiveItem)
                {
                    //Spawn a new passive item
                    upgradeOption.UpgradeButton.onClick.AddListener(() => _player.SpawnPassiveItem(selectedPassiveItemUpgrade.InitialPassiveItem));
                    upgradeOption.UpgradeNameDisplay.text = selectedPassiveItemUpgrade.PassiveItemData.PassiveName;
                    upgradeOption.UpgradeDescriptionDisplay.text = selectedPassiveItemUpgrade.PassiveItemData.Description;
                }
                upgradeOption.UpgradeIcon.sprite = selectedPassiveItemUpgrade.PassiveItemData.Icon;
            }
        }
    }

    private void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in UpgradeUIOptions)
        {
            upgradeOption.UpgradeButton.onClick.RemoveAllListeners(); // Remove all listeners from the button
            upgradeOption.UpgradeNameDisplay.text = string.Empty; // Clear the name display
            upgradeOption.UpgradeDescriptionDisplay.text = string.Empty; // Clear the description display
            upgradeOption.UpgradeIcon.sprite = null; // Clear the icon
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions(); // Remove existing upgrade options
        ApplyUpgradeOptions(); // Apply new upgrade options
    }
}

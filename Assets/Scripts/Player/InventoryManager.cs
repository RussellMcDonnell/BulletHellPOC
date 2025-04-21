using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

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

    public List<WeaponEvolutionBlueprint> WeaponEvolutionOptions = new List<WeaponEvolutionBlueprint>();

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
        // Create temporary lists to store the upgrade options. Exclude options that are already at max level (NextLevelPrefab is null)
        List<WeaponUpgrade> availableWeaponUpgradeOptions = new List<WeaponUpgrade>(WeaponUpgradeOptions.Where(x => x.WeaponData.NextLevelPrefab != null));
        List<PassiveItemUpgrade> availablePassiveItemUpgradeOptions = new List<PassiveItemUpgrade>(PassiveItemUpgradeOptions.Where(x => x.PassiveItemData.NextLevelPrefab != null));

        foreach (var upgradeOption in UpgradeUIOptions)
        {
            // Check if there are available upgrade options
            if (availableWeaponUpgradeOptions.Count == 0 && availablePassiveItemUpgradeOptions.Count == 0)
            {
                Debug.Log("No available upgrade options to apply.");
                // Disable the upgrade option UI
                DisableUpgradeUI(upgradeOption);
                continue; // Exit the loop if no upgrade options are available
            }

            // There are available upgrade options, enable the UI
            EnableUpgradeUI(upgradeOption);

            int upgradeType;

            // if there are no available upgrade options of one type, force the other type
            if (availableWeaponUpgradeOptions.Count == 0)
            {
                upgradeType = 2; // Force passive item upgrade
            }
            else if (availablePassiveItemUpgradeOptions.Count == 0)
            {
                upgradeType = 1; // Force weapon upgrade
            }
            else
            {
                // Both types are available, randomly select one
                upgradeType = UnityEngine.Random.Range(1, 3); // Min inclusive, Max exclusive (1 for weapon, 2 for passive item)
            }


            if (upgradeType == 1) // Weapon upgrade
            {
                // Select a random weapon upgrade option
                int randomIndex = UnityEngine.Random.Range(0, availableWeaponUpgradeOptions.Count);
                WeaponUpgrade selectedWeaponUpgrade = availableWeaponUpgradeOptions[randomIndex];

                // Remove the selected upgrade option from the temporary list so it won't be selected again
                availableWeaponUpgradeOptions.Remove(selectedWeaponUpgrade);

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
                int randomIndex = UnityEngine.Random.Range(0, availablePassiveItemUpgradeOptions.Count);
                PassiveItemUpgrade selectedPassiveItemUpgrade = availablePassiveItemUpgradeOptions[randomIndex];

                // Remove the selected upgrade option from the temporary list so it won't be selected again
                availablePassiveItemUpgradeOptions.Remove(selectedPassiveItemUpgrade);

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

    private void DisableUpgradeUI(UpgradeUI upgradeOption)
    {
        // Disable the parent game object of the upgrade option
        upgradeOption.UpgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }
    private void EnableUpgradeUI(UpgradeUI upgradeOption)
    {
        // Enable the parent game object of the upgrade option
        upgradeOption.UpgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }


    private void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in UpgradeUIOptions)
        {
            upgradeOption.UpgradeButton.onClick.RemoveAllListeners(); // Remove all listeners from the button
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions(); // Remove existing upgrade options
        ApplyUpgradeOptions(); // Apply new upgrade options
    }

    public List<WeaponEvolutionBlueprint> GetPossibleEvolutions()
    {
        var possibleEvolutions = new List<WeaponEvolutionBlueprint>();

        var availableWeapons = WeaponSlots.Where(w => w != null).ToList();
        var availableCatalysts = PassiveItemSlots.Where(p => p != null).ToList();

        foreach (var evolution in WeaponEvolutionOptions)
        {
            foreach (var weapon in availableWeapons)
            {
                if (weapon.WeaponData.Level < evolution.BaseWeaponData.Level)
                    continue;

                foreach (var catalyst in availableCatalysts)
                {
                    if (catalyst.PassiveItemData.Level >= evolution.CatalystPassiveItemData.Level)
                    {
                        possibleEvolutions.Add(evolution);
                        break; // No need to keep checking catalysts if one matches
                    }
                }
            }
        }

        return possibleEvolutions;
    }


    // TODO there is an issue where we are not checking the type of the weapon and passive item just the level
    public void EvolveWeapon(WeaponEvolutionBlueprint evolution)
    {
        for (int weaponSlotIndex = 0; weaponSlotIndex < WeaponSlots.Count; weaponSlotIndex++)
        {
            WeaponController weapon = WeaponSlots[weaponSlotIndex];
            if (!weapon)
                continue;

            for (int catalystSlotIndex = 0; catalystSlotIndex < PassiveItemSlots.Count; catalystSlotIndex++)
            {
                PassiveItem catalyst = PassiveItemSlots[catalystSlotIndex];
                if (!catalyst)
                    continue;

                // Check if the weapon and passive item match the evolution criteria
                if (weapon.WeaponData.Level >= evolution.BaseWeaponData.Level && catalyst.PassiveItemData.Level >= evolution.CatalystPassiveItemData.Level)
                {
                    GameObject evolvedWeapon = Instantiate(evolution.EvolvedWeapon, transform.position, Quaternion.identity);
                    WeaponController evolvedWeaponController = evolvedWeapon.GetComponent<WeaponController>();

                    evolvedWeapon.transform.SetParent(transform); // Set the parent of the spawned weapon to the player
                    AddWeapon(weaponSlotIndex, evolvedWeaponController); // Add the evolved weapon to the inventory slot
                    Destroy(weapon.gameObject); // Destroy the old weapon

                    //Update level and icon
                    WeaponLevels[weaponSlotIndex] = evolvedWeaponController.WeaponData.Level; // Update the weapon level
                    WeaponIcons[weaponSlotIndex].sprite = evolvedWeaponController.WeaponData.Icon; // Update the icon

                    WeaponUpgradeOptions.RemoveAt(evolution.BaseWeaponData.EvolvedUpgradeToRemove); // Remove the weapon upgrade option that was used for the evolution

                    Debug.Log("Evolved " + weapon.WeaponData.WeaponName + " into " + evolvedWeaponController.WeaponData.WeaponName);

                    return;
                }
            }
        }
    }
}

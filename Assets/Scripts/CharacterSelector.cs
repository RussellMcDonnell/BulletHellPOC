using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance { get; private set; } // Singleton instance of the CharacterSelector
    public CharacterScriptableObject CharacterData;

    private void Awake()
    {
        // Ensure that only one instance of CharacterSelector exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Debug.LogWarning("Duplicate " + this + " found. Destroying duplicate.");
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    
    public static CharacterScriptableObject GetCharacterData()
    {
        return Instance.CharacterData; // Return the character data from the singleton instance
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        CharacterData = character; // Set the selected character data
        Debug.Log("Selected character: " + character.name); // Log the selected character
    }

    public void DestroySingleton()
    {
        Instance = null; // Reset the singleton instance
        Destroy(gameObject); // Destroy the singleton instance
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    private bool _isGameOver = false; // Flag to check if the game is over

    public GameState CurrentState;

    public GameState PreviousState;

    [Header("Screens")]
    public GameObject PauseScreen;
    public GameObject ResultsScreen;


    //Current stats display text
    [Header("Current Stats Display")]
    public TextMeshProUGUI CurrentHealthDisplay;
    public TextMeshProUGUI CurrentRecoveryDisplay;
    public TextMeshProUGUI CurrentMovementSpeedDisplay;
    public TextMeshProUGUI CurrentMightDisplay;
    public TextMeshProUGUI CurrentProjectileSpeedDisplay;
    public TextMeshProUGUI CurrentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image ChosenCharacterImage;
    public TextMeshProUGUI ChosenCharacterName;


    private void Awake()
    {
        // Check if an instance of GameManager already exists
        if (Instance == null)
        {
            Instance = this; // Set the instance to this GameManager
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene
        }
        else if (this != null && Instance != this)
        {
            Instance = this; // Set the instance to this GameManager
            // Destroy(gameObject); // Destroy any duplicate in new scenes
        }

        DisableScreens();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (CurrentState)
        {
            case GameState.MainMenu:
                // Handle main menu logic
                break;
            case GameState.Playing:
                CheckForPauseAndResume();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                // Check if the game is over and display the results screen
                if (!_isGameOver)
                {
                    _isGameOver = true; // Set the game over flag to true
                    DisplayResults(); // Display the results screen
                }
                break;
            default:
                Debug.LogError("Unknown game state: " + CurrentState);
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        PreviousState = CurrentState; // Store the previous state
        CurrentState = newState;
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Paused)
        {
            UpdatePauseScreenCurrentStats();
            PauseScreen.SetActive(true); // Show the pause screen
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            PauseScreen.SetActive(false); // hide the pause screen
            ChangeState(PreviousState);
            Time.timeScale = 1f; // Resume the game
            Debug.Log("Game Resumed");
        }
    }

    private void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Updates the current stats displayed on the pause screen.
    /// </summary>
    private void UpdatePauseScreenCurrentStats()
    {
        // Get the Player component by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // Get the player stats from the player component
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        // Update the UI text elements with the player's current stats
        CurrentHealthDisplay.text = "Health: " + playerStats.CurrentHealth.ToString("F0") + "/" + playerStats.CurrentMaxHealth.ToString("F0");
        CurrentRecoveryDisplay.text = "Recovery: " + playerStats.CurrentRecovery.ToString("F1") + "/s";
        CurrentMovementSpeedDisplay.text = "Movement Speed: " + playerStats.CurrentMovementSpeed.ToString("F0");
        CurrentMightDisplay.text = "Might: " + playerStats.CurrentMight.ToString("F0");
        CurrentProjectileSpeedDisplay.text = "Projectile Speed: " + playerStats.CurrentProjectileSpeed.ToString("F0");
        CurrentMagnetDisplay.text = "Magnet: " + playerStats.CurrentMagnet.ToString("F0");
    }

    private void DisableScreens()
    {
        PauseScreen.SetActive(false);
        ResultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        // Handle game over logic here (e.g., show game over screen, reset game, etc.)
        Debug.Log("Game Over");
        Time.timeScale = 0f; // Pause the game
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        ResultsScreen.SetActive(true); // Show the results screen
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject characterData)
    {
        ChosenCharacterImage.sprite = characterData.CharacterSprite; // Assign the character image to the UI
        ChosenCharacterName.text = characterData.CharacterName; // Assign the character name to the UI
    }

    private void AssignRuntimeReferences()
    {
        PauseScreen = GameObject.Find("Pause Screen");
        ResultsScreen = GameObject.Find("Results Screen");

        // Example of finding TextMeshProUGUI by name
        CurrentHealthDisplay = GameObject.Find("Current Health Display")?.GetComponent<TextMeshProUGUI>();
        CurrentRecoveryDisplay = GameObject.Find("Current Recovery Display")?.GetComponent<TextMeshProUGUI>();
        CurrentMovementSpeedDisplay = GameObject.Find("Current Move Speed Display")?.GetComponent<TextMeshProUGUI>();
        CurrentMightDisplay = GameObject.Find("Current Might Display")?.GetComponent<TextMeshProUGUI>();
        CurrentProjectileSpeedDisplay = GameObject.Find("Current Projectile Speed Display")?.GetComponent<TextMeshProUGUI>();
        CurrentMagnetDisplay = GameObject.Find("Current Magnet Display")?.GetComponent<TextMeshProUGUI>();

        ChosenCharacterImage = GameObject.Find("Chosen Character Image")?.GetComponent<Image>();
        ChosenCharacterName = GameObject.Find("Chosen Character Name")?.GetComponent<TextMeshProUGUI>();
    }
}

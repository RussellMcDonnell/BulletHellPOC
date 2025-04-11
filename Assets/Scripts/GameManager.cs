using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState;

    public GameState PreviousState;

    [Header("UI")]
    public GameObject PauseScreen;

    //Current stats display text
    public TextMeshProUGUI CurrentHealthDisplay;
    public TextMeshProUGUI CurrentRecoveryDisplay;
    public TextMeshProUGUI CurrentMovementSpeedDisplay;
    public TextMeshProUGUI CurrentMightDisplay;
    public TextMeshProUGUI CurrentProjectileSpeedDisplay;
    public TextMeshProUGUI CurrentMagnetDisplay;



    private void Awake()
    {
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
                // Handle game over logic
                break;
            default:
                Debug.LogError("Unknown game state: " + CurrentState);
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Paused)
        {
            UpdatePauseScreenCurrentStats();
            PauseScreen.SetActive(true); // Show the pause screen
            PreviousState = CurrentState;
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
    }
}

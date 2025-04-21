using System.Collections;
using System.Collections.Generic;
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
        LevelingUp,
        Paused,
        GameOver
    }

    private bool _isGameOver = false; // Flag to check if the game is over
    public bool ChoosingUpgrade = false; // Flag to check if the player is choosing an upgrade

    public GameState CurrentState;

    public GameState PreviousState;

    [Header("Damage Text Settings")]
    public Canvas DamageTextCanvas;
    public float TextFontSize = 50f;
    public TMP_FontAsset TextFont;
    public Camera ReferenceCamera;

    [Header("Screens")]
    public GameObject PauseScreen;
    public GameObject ResultsScreen;
    public GameObject LevelUpScreen;


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
    public TextMeshProUGUI LevelReachedDisplay;
    public TextMeshProUGUI TimeSurvivedDisplay;
    public List<Image> ChosenPassiveItemsImages = new List<Image>(6);
    public List<Image> ChosenWeaponsImages = new List<Image>(6);

    [Header("Stopwatch")]
    public float StopwatchTime = 0f; // Time survived in the game
    public float TimeLimit = 0f;
    public TextMeshProUGUI StopwatchDisplay;

    public GameObject PlayerObject;


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
                // Update the time survived
                UpdateStopwatch();
                CheckForPauseAndResume();
                break;
            case GameState.LevelingUp:
                // Handle leveling up logic
                if (!ChoosingUpgrade)
                {
                    ChoosingUpgrade = true;
                    Time.timeScale = 0f; // Pause the game
                    LevelUpScreen.SetActive(true); // Show the level up screen
                }
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

    private void UpdateStopwatch()
    {
        StopwatchTime += Time.deltaTime; // Increment the time survived by the time since the last frame
        UpdateStopwatchDisplay();

        if (StopwatchTime >= TimeLimit) // Check if the time limit is reached
        {
            GameOver(); // Call the game over method
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
        LevelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        // Handle game over logic here (e.g., show game over screen, reset game, etc.)
        Debug.Log("Game Over");
        Time.timeScale = 0f; // Pause the game
        // Check for null reference before updating the display
        if (StopwatchDisplay?.text != null)
        {
            TimeSurvivedDisplay.text = StopwatchDisplay.text; // Assign the time survived to the display text
        }
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        UpdateStopwatchDisplay();
        ResultsScreen.SetActive(true); // Show the results screen
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject characterData)
    {
        ChosenCharacterImage.sprite = characterData.CharacterSprite; // Assign the character image to the UI
        ChosenCharacterName.text = characterData.CharacterName; // Assign the character name to the UI
    }

    public void AssignLevelReachedUI(int levelReached)
    {
        if (LevelReachedDisplay != null)
        {
            LevelReachedDisplay.text = levelReached.ToString();
        }
    }

    public void AssignChosenWeaponsAndPassivesItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        // check if lists are the same size
        if (chosenWeaponsData.Count != ChosenWeaponsImages.Count || chosenPassiveItemsData.Count != ChosenPassiveItemsImages.Count)
        {
            Debug.LogError("The number of weapons and passive items does not match the UI slots.");
        }

        for (int i = 0; i < chosenWeaponsData.Count; i++)
        {
            if (chosenWeaponsData[i]?.sprite != null)
            {
                ChosenWeaponsImages[i].enabled = true; // Enable the image slot for the weapon
                ChosenWeaponsImages[i].sprite = chosenWeaponsData[i].sprite; // Assign the weapon images to the UI
            }
            else
            {
                ChosenWeaponsImages[i].enabled = false; // Disable the image slot for the weapon if no sprite is assigned
            }
        }

        for (int i = 0; i < chosenPassiveItemsData.Count; i++)
        {
            if (chosenPassiveItemsData[i]?.sprite != null)
            {
                ChosenPassiveItemsImages[i].enabled = true; // Enable the image slot for the passive item
                ChosenPassiveItemsImages[i].sprite = chosenPassiveItemsData[i].sprite; // Assign the passive item images to the UI
            }
            else
            {
                ChosenPassiveItemsImages[i].enabled = false; // Disable the image slot for the passive item if no sprite is assigned
            }
        }
    }

    public void UpdateStopwatchDisplay()
    {
        if (TimeSurvivedDisplay != null)
        {
            // Format the time
            int minutes = Mathf.FloorToInt(StopwatchTime / 60f);
            int seconds = Mathf.FloorToInt(StopwatchTime % 60f);
            StopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartLevelingUp()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.LevelingUp); // Change the game state to leveling up
            Time.timeScale = 0f; // Pause the game
            LevelUpScreen.SetActive(true); // Show the level up screen
            PlayerObject.SendMessage("RemoveAndApplyUpgrades");
        }
    }

    public void EndLevelingUp()
    {
        if (CurrentState == GameState.LevelingUp)
        {
            LevelUpScreen.SetActive(false); // Hide the level up screen
            ChangeState(GameState.Playing); // Change the game state back to playing
            Time.timeScale = 1f; // Resume the game
            ChoosingUpgrade = false; // Reset the choosing upgrade flag
        }
    }

    public static void GenerateFloatingText(string text, Transform target, float duration = 0.75f, float speed = 1f)
    {
        // If the canvas is not set, end the function so we don't generate any floating text
        if (!Instance.DamageTextCanvas)
        {
            return;
        }

        // Find a relevant camera that we can use to convert the world position to screen position
        if (!Instance.ReferenceCamera)
        {
            Instance.ReferenceCamera = Camera.main;
        }

        Instance.StartCoroutine(Instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }

    private IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 0.75f, float speed = 1f)
    {
        // Create a new TextMeshPro object for the floating text
        GameObject floatingText = new GameObject("FloatingText");
        RectTransform rectTransform = floatingText.AddComponent<RectTransform>();
        TextMeshProUGUI textMesh = floatingText.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.fontSize = TextFontSize;
        if (TextFont)
        {
            textMesh.font = TextFont;
        }
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.verticalAlignment = VerticalAlignmentOptions.Middle;
        textMesh.color = Color.white;

        // Safety check: Make sure target isn't null before setting position
        if (target != null)
        {
            rectTransform.position = ReferenceCamera.WorldToScreenPoint(target.position);
        }

        // Destroy the floating text object after the animation is complete
        Destroy(floatingText, duration);

        floatingText.transform.SetParent(Instance.DamageTextCanvas.transform);

        // Pan the text upwards and fade it away over time
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float t = 0f;
        float yOffset = 0f;
        while (t < duration)
        {
            yield return waitForEndOfFrame; // Wait for the end of the frame
            t += Time.deltaTime; // Increment the time variable

            // Fade out the text color to the right alpha value
            textMesh.color = Color.Lerp(Color.white, Color.clear, t / duration);

            // Pan the text upwards
            yOffset += speed * Time.deltaTime; // Calculate the vertical offset

            if(!rectTransform)
            {
                yield break; // Exit the coroutine if rectTransform is null
            }

            if (target != null)
            {
                rectTransform.position = ReferenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
            }
            else
            {
                // Optional: If the target is gone, maybe just move the text upwards from its last position
                rectTransform.position += new Vector3(0, speed * Time.deltaTime);
            }
        }
    }
}

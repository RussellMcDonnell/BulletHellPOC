using UnityEngine;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if(CurrentState != GameState.Paused)
        {
            PreviousState = CurrentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if(CurrentState == GameState.Paused)
        {
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
}

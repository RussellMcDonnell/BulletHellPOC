using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f; // Ensure the game is unpaused when changing scenes

        switch (sceneName)
        {
            case "Menu":
                GameManager.Instance?.ChangeState(GameManager.GameState.MainMenu);
                break;
            case "Game":
                if (GameManager.Instance != null)
                {
                    GameManager.Instance?.ChangeState(GameManager.GameState.Playing);
                    GameManager.Instance.TimeSurvived = 0; // Reset the time survived
                }
                break;
        }
    }
}

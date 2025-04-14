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
            case "MainMenu":
                GameManager.Instance.ChangeState(GameManager.GameState.MainMenu); // Set the game state to MainMenu
                break;
            case "Game":
                // GameManager.Instance.ChangeState(GameManager.GameState.Playing); // Set the game state to Game
                break;
        }
    }
}

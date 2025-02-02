using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Replace "GameScene" with your actual scene name
    }

    public void ExitGame()
    {
        Application.Quit(); // Quits the game
        Debug.Log("Game has exited."); // Debug message for editor testing
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
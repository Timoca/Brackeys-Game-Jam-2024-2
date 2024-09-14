using UnityEngine;
using UnityEngine.SceneManagement;

public class Redirector : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadHighScoresScene()
    {
        SceneManager.LoadScene("HighScores");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int score = 0;
    public List<int> highScores = new List<int>();

    private const int maxHighScores = 5;

    private bool _hasUpdatedHighScores = false;

    void Start()
    {
        LoadHighScores();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void SaveHighScore()
    {
        if (_hasUpdatedHighScores == false)
        {
            // Add the current score to the list of high scores
            highScores.Add(score);

            // Sort the list in descending order
            highScores.Sort((a, b) => b.CompareTo(a));

            // Ensure only the top 5 scores are kept
            if (highScores.Count > maxHighScores)
            {
                highScores.RemoveAt(maxHighScores); // Remove the lowest score if the list is longer than 5
            }

            // Save the top 5 scores in PlayerPrefs
            for (int i = 0; i < highScores.Count; i++)
            {
                PlayerPrefs.SetInt("HighScore" + i, highScores[i]);
            }
            PlayerPrefs.Save();
            Debug.Log("Highscores saved");
            _hasUpdatedHighScores = true;
        }
    }

    private void LoadHighScores()
    {
        highScores.Clear();
        for (int i = 0; i < maxHighScores; i++)
        {
            if (PlayerPrefs.HasKey("HighScore" + i))
            {
                highScores.Add(PlayerPrefs.GetInt("HighScore" + i));
            }
        }
    }

    public List<int> GetHighScores()
    {
        return highScores;
    }
}

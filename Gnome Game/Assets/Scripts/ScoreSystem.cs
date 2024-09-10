using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int score = 0;
    private List<int> highScores = new List<int>();

    private const int maxHighScores = 5;

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
        // Voeg de huidige score toe aan de lijst met highscores
        highScores.Add(score);

        // Sorteer de lijst van hoog naar laag
        highScores.Sort((a, b) => b.CompareTo(a));

        // Zorg ervoor dat alleen de top 5 scores worden bewaard
        if (highScores.Count > maxHighScores)
        {
            highScores.RemoveAt(maxHighScores); // Verwijder de laagste score als de lijst langer is dan 5
        }

        // Sla de top 5 scores op in PlayerPrefs
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, highScores[i]);
        }

        PlayerPrefs.Save();
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

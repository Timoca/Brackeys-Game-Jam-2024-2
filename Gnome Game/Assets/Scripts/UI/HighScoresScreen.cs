using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoresScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text Score1;
    [SerializeField] private TMP_Text Score2;
    [SerializeField] private TMP_Text Score3;
    [SerializeField] private TMP_Text Score4;
    [SerializeField] private TMP_Text Score5;

    private ScoreSystem _scoreSystem;

    void Start()
    {
        _scoreSystem = GetComponent<ScoreSystem>();

        List<int> highScores = _scoreSystem.GetHighScores();

        if (highScores[0] != 0)
        {
            Score1.text = $"1: {highScores[0]}";
        }
        if (highScores[1] != 0)
        {
            Score2.text = $"2: {highScores[1]}";
        }
        if (highScores[2] != 0)
        {
            Score3.text = $"3: {highScores[2]}";
        }
        if (highScores[3] != 0)
        {
            Score4.text = $"4: {highScores[3]}";
        }
        if (highScores[4] != 0)
        {
            Score5.text = $"5: {highScores[4]}";
        }

    }
}

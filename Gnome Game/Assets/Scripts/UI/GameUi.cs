using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text topHighScoreText;
    private ScoreSystem _scoreSystem;
    void Start()
    {
        _scoreSystem = FindAnyObjectByType<ScoreSystem>();

        if (_scoreSystem.GetHighScores().Count == 0)
        {
            topHighScoreText.text = "Top Highscore: 0";
        }
        else
        {
            topHighScoreText.text = "Top Highscore: " + _scoreSystem.GetHighScores()[0];
        }
    }


    void Update()
    {
        scoreText.text = "Score: " + _scoreSystem.score;
    }
}

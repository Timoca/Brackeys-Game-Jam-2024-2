using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text topHighScoreText;
    [SerializeField] TMP_Text timeText;
    private ScoreSystem _scoreSystem;
    private GameTimer _gameTimer;
    void Start()
    {
        _scoreSystem = FindAnyObjectByType<ScoreSystem>();
        _gameTimer = FindAnyObjectByType<GameTimer>();


    }


    void Update()
    {
        scoreText.text = "Score: " + _scoreSystem.score;

        timeText.text = "Time: " + _gameTimer.timeRemaining.ToString();
        if (_scoreSystem.GetHighScores().Count == 0)
        {
            topHighScoreText.text = "Top Highscore: 0";
            Debug.LogWarning("No highscores found");
        }
        else
        {
            topHighScoreText.text = "Top Highscore: " + _scoreSystem.GetHighScores()[0];
        }
    }
}

using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] Canvas _gameOverCanvas;
    public int lengthOfGame = 60; // Time in seconds
    public int timeRemaining; // Time in seconds

    public bool gameEnded = false;

    private ScoreSystem _scoreSystem;
    void Start()
    {
        _scoreSystem = GetComponent<ScoreSystem>();
        timeRemaining = lengthOfGame;
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {

        if (timeRemaining <= 0)
        {
            EndGame();
        }
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            timeRemaining--;
        }
    }

    private void EndGame()
    {
        StartCoroutine(SaveHighScore());
        _gameOverCanvas.gameObject.SetActive(true);
        gameEnded = true;
    }

    private IEnumerator SaveHighScore()
    {
        yield return new WaitForSeconds(1.5f);
        _scoreSystem.SaveHighScore();
    }
}

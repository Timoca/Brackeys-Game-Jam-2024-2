using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] int lengthOfGame = 60; // Time in seconds
    public int timeRemaining; // Time in seconds

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
            Debug.Log("Game Over");
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
        _scoreSystem.SaveHighScore();
    }
}

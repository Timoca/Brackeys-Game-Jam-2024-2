using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    private ScoreSystem _scoreSystem;
    void Start()
    {
        _scoreSystem = FindAnyObjectByType<ScoreSystem>();
    }


    void Update()
    {
        scoreText.text = "Score: " + _scoreSystem.score;
    }
}

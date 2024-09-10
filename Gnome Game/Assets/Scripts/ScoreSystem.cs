using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int score = 0;

    public void AddScore(int amount)
    {
        score += amount;
    }
}

using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private CollectibleSpawner _collectibleSpawner;
    private ScoreSystem _scoreSystem;

    private bool _HasAddedScore = false;

    void Start()
    {
        _collectibleSpawner = FindAnyObjectByType<CollectibleSpawner>();
        _scoreSystem = FindAnyObjectByType<ScoreSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _HasAddedScore = false;
        foreach (var item in _collectibleSpawner.collectibles)
        {
            if (item.objectTag == other.tag && !_HasAddedScore)
            {
                Debug.Log("Item collected: " + item.objectTag);
                Debug.Log("Score: " + item.score);
                _scoreSystem.AddScore(item.score);
                Destroy(other.gameObject);
                _HasAddedScore = true;
            }
        }
    }
}

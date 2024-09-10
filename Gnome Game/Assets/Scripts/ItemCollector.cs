using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private CollectibleSpawner _collectibleSpawner;
    private ScoreSystem _scoreSystem;

    void Start()
    {
        _collectibleSpawner = FindAnyObjectByType<CollectibleSpawner>();
        _scoreSystem = FindAnyObjectByType<ScoreSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in _collectibleSpawner.collectibles)
        {
            if (item.objectTag == other.tag)
            {
                _scoreSystem.AddScore(item.score);
                Destroy(other.gameObject);
            }
        }
    }
}

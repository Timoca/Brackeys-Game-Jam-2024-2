using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private CollectibleSpawner _collectibleSpawner;
    private ScoreSystem _scoreSystem;
    private WheelbarrowAudio _wheelbarrowAudio;

    private bool _HasAddedScore = false;

    void Start()
    {
        _collectibleSpawner = FindAnyObjectByType<CollectibleSpawner>();
        _scoreSystem = FindAnyObjectByType<ScoreSystem>();
        _wheelbarrowAudio = FindAnyObjectByType<WheelbarrowAudio>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _HasAddedScore = false;
        foreach (var item in _collectibleSpawner.collectibles)
        {
            if (item.objectTag == other.tag && !_HasAddedScore)
            {
                _scoreSystem.AddScore(item.score);
                _wheelbarrowAudio.PlayCollectingSound();
                Destroy(other.gameObject);
                _HasAddedScore = true;
            }
        }
    }
}

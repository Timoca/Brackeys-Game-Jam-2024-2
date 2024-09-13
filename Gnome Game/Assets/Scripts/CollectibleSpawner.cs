using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public List<Collectible> collectibles;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private Vector2 spawnRange = new Vector2(-10f, 10f);
    public float spawnInterval = 1.0f;

    private GameTimer _gameTimer;
    private Coroutine _spawnCoroutine;
    void Start()
    {
        _gameTimer = FindAnyObjectByType<GameTimer>();

        _spawnCoroutine = StartCoroutine(SpawnCollectiblesCoroutine());
    }

    void Update()
    {
        if (_gameTimer.gameEnded && _spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnCollectiblesCoroutine()
    {
        while (true)
        {
            // Genereer een willekeurige positie langs een lijn over de X- en Z-assen
            float randomZ = Random.Range(spawnRange.x, spawnRange.y);

            Vector3 spawnPosition = new Vector3(0, spawnHeight, randomZ);
            GameObject CollectibleToSpawn = collectibles[Random.Range(0, collectibles.Count)].prefab;
            // Spawn het collectible object
            Instantiate(CollectibleToSpawn, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

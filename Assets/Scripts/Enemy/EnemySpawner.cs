using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float minDelay = 3f;
    [SerializeField] private float maxDelay = 5f;
    [SerializeField] private int maxEnemies = 2;

    [SerializeField] private Enemy[] enemiesPrefabs;
    
    [SerializeField] private Wormhole wormholePrefab;
    [SerializeField] private float wormholeMinTravelTime = 0.5f;
    [SerializeField] private float wormholeMaxTravelTime = 1f;
    
    [SerializeField] private float overflow = 0f;
    
    private float _xMin;
    private float _xMax;

    private float _yMin;
    private float _yMax;

    private bool _isSpawning;
    
    private void Awake()
    {
        EventManager.AddListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.AddListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.AddListener(Events.NET_BROKEN, OnNetDestroyed);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.RemoveListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.RemoveListener(Events.NET_BROKEN, OnNetDestroyed);
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }
    
    void OnBoundariesBottomLeft(Vector3 vector)
    {
        _xMin = vector.x + overflow;
        _yMin = vector.y + overflow;
    }

    void OnBoundariesTopRight(Vector3 vector)
    {
        _xMax = vector.x - overflow;
        _yMax = vector.y - overflow;
    }

    void OnNetDestroyed()
    {
        if (!_isSpawning)
        {
            _isSpawning = true;
            StartCoroutine(StartSpawning());
        }
    }

    void OnLevelFinished()
    {
        _isSpawning = false;
    }

    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(initialDelay);

        while (_isSpawning)
        {
            if (!LimitOfEnemiesReached())
                yield return StartCoroutine(SpawnEnemy());

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    bool LimitOfEnemiesReached()
    {
        return GetComponentsInChildren<Enemy>().Length >= maxEnemies;
    }

    IEnumerator SpawnEnemy()
    {
        Vector2 position = new Vector2(Random.Range(_xMin, _xMax), Random.Range(_yMin, _yMax));

        Wormhole wormhole = Instantiate(wormholePrefab,
            position,
            Quaternion.identity,
            transform);
        
        yield return new WaitForSeconds(Random.Range(wormholeMinTravelTime, wormholeMaxTravelTime));

        Instantiate(
            enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)],
            position,
            Quaternion.identity,
            transform);
        
        wormhole.Close();
    }
}

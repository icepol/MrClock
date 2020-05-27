using System.Collections;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private float maxSpawnDelay = 1f;
    [SerializeField] private Boss bossPrefab;
    
    [SerializeField] private Wormhole wormholePrefab;
    [SerializeField] private float wormholeMinTravelTime = 0.5f;
    [SerializeField] private float wormholeMaxTravelTime = 1f;
    
    [SerializeField] private float overflow = 0f;
    
    private float _xMin;
    private float _xMax;

    private float _yMin;
    private float _yMax;
    
    void Awake()
    {
        EventManager.AddListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.AddListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.AddListener(Events.NET_BROKEN, OnNetDestroyed);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.RemoveListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.RemoveListener(Events.NET_BROKEN, OnNetDestroyed);
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

    private void OnNetDestroyed()
    {
        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(Random.Range(0f, maxSpawnDelay));
        
        Vector2 position = new Vector2(Random.Range(_xMin, _xMax), Random.Range(_yMin, _yMax));
        
        Wormhole wormhole = Instantiate(wormholePrefab, position, Quaternion.identity);
        
        yield return new WaitForSeconds(Random.Range(wormholeMinTravelTime, wormholeMaxTravelTime));

        Instantiate(bossPrefab, position, Quaternion.identity);
        
        wormhole.Close();
        
        Destroy(gameObject);
    }
}

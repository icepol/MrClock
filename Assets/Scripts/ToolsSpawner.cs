using System.Collections;
using UnityEngine;

public class ToolsSpawner : MonoBehaviour
{
    [SerializeField] private float minDelay = 2;
    [SerializeField] private float maxDelay = 4;
    
    [SerializeField] private Tool[] toolsPrefabs;
    
    [SerializeField] private float overflow = 0f;
    
    private float _xMin;
    private float _xMax;

    private float _yMin;
    private float _yMax;

    private bool _isSpawning;
    
    private int _toolsRequired = 0;
    private int _toolsSpawned = 0;
    private int _toolsCollected = 0;
    
    private void Awake()
    {
        EventManager.AddListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.AddListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.AddListener(Events.NET_BROKEN, OnNetDestroyed);
        EventManager.AddListener(Events.TOOL_SPAWNED, OnToolSpawned);
        EventManager.AddListener(Events.TOOL_COLLECTED, OnToolCollected);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void Start()
    {
        _isSpawning = true;
        StartCoroutine(StartSpawning());
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.RemoveListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.RemoveListener(Events.NET_BROKEN, OnNetDestroyed);
        EventManager.RemoveListener(Events.TOOL_SPAWNED, OnToolSpawned);
        EventManager.RemoveListener(Events.TOOL_COLLECTED, OnToolCollected);
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
        _toolsRequired++;
    }

    void OnToolSpawned()
    {
        _toolsSpawned++;
    }

    void OnToolCollected()
    {
        _toolsCollected++;
    }
    
    void OnLevelFinished()
    {
        _isSpawning = false;
    }

    IEnumerator StartSpawning()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(1f);

            if (_toolsSpawned >= _toolsRequired || _toolsCollected < _toolsSpawned)
                // we didn't need more tools now
                continue;
            
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            SpawnTool();
        }
    }
    
    void SpawnTool()
    {
        Vector2 position = new Vector2(Random.Range(_xMin, _xMax), Random.Range(_yMin, _yMax));
        Tool tool = Instantiate(
            toolsPrefabs[Random.Range(0, toolsPrefabs.Length)],
            position,
            Quaternion.identity,
            transform);
    }
}

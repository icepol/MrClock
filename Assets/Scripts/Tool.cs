using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] private float boundariesDistance = 0.5f;
    [SerializeField] private int collectScorePoints = 5;

    [SerializeField] private ScoreBalloon scoreBalloonPrefab;
 
    private Vector2 _targetPosition;

    private void Start()
    {
        EventManager.TriggerEvent(Events.TOOL_SPAWNED);
        
        SetTargetPosition();
    }

    private void Update()
    {
        Vector2 position = Vector2.MoveTowards(
            transform.position, 
            _targetPosition, 
            moveSpeed * Time.deltaTime
            );

        transform.position = position;

        if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
            SetTargetPosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
            Collect();

        Wall wall = other.GetComponent<Wall>();
        if (wall)
            SetTargetPosition();
    }

    void Collect()
    {
        EventManager.TriggerEvent(Events.TOOL_COLLECTED);
        
        int score = GameState.Level * collectScorePoints;
        
        GameState.Score += score;
        SpawnScoreBalloon(score);
        
        Destroy(gameObject);
    }

    void SetTargetPosition()
    {
        _targetPosition = new Vector2(
            Random.Range(GameState.MinX + boundariesDistance, GameState.MaxX - boundariesDistance), 
            Random.Range(GameState.MinY + boundariesDistance, GameState.MaxY - boundariesDistance)
            );
    }
    
    void SpawnScoreBalloon(int score)
    {
        ScoreBalloon scoreBalloon = Instantiate(scoreBalloonPrefab, transform.position, Quaternion.identity);
        scoreBalloon.SetScore(score);
    }
}

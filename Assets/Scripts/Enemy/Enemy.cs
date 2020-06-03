using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamage
{
    [SerializeField] float delayBetweenState = 1f;
    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private GameObject explosion;

    [SerializeField] private int lives = 1;
    [SerializeField] private int hitScorePoints = 1;
    [SerializeField] private int killScorePoints = 1;

    [SerializeField] private ScoreBalloon scoreBalloonPrefab;
    
    public bool IsSpawningFinished { get; set; }

    private Animator _animator;
    
    private Vector2 _targetPosition;
    private Player _player;

    public enum EnemyState
    {
        Spawned,
        LookingForPlayer,
        ChasingPlayer,
        AttackPlayer,
        Observing,
        Freezed
    }

    public EnemyState State { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _player = FindObjectOfType<Player>();

        StartCoroutine(NextState());
        
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }
    
    void Update()
    {
        if (State == EnemyState.ChasingPlayer || State == EnemyState.Observing)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _targetPosition, 
                moveSpeed * Time.deltaTime
                );
            
            transform.localScale = new Vector2(transform.position.x < _targetPosition.x ? 1 : -1, 1);
            
            _animator.SetBool("IsMoving", true);
        }
        else
            _animator.SetBool("IsMoving", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
            CollisionWithPlayer(player);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
            CollisionWithPlayer(player);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied); 
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    IEnumerator NextState()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenState);

            switch (State)
            {
                case EnemyState.Spawned:
                    Spawned();
                    break;
                case EnemyState.LookingForPlayer:
                    LookingForPlayer();
                    break;
                case EnemyState.ChasingPlayer:
                    ChasingPlayer();
                    break;
                case EnemyState.AttackPlayer:
                    AttackPlayer();
                    break;
                case EnemyState.Observing:
                    Observing();
                    break;
            }
        }
    }
    
    void CollisionWithPlayer(Player player)
    {
            
        if (State != EnemyState.Spawned && State != EnemyState.AttackPlayer)
        {
            State = EnemyState.AttackPlayer;
            
            EventManager.TriggerEvent(Events.PLAYER_UNDER_ATTACK);
        }
    }

    void Spawned()
    {
        if (IsSpawningFinished)
            // once the spawning is finished we can go to next state
            State = EnemyState.LookingForPlayer;
        else
            IsSpawningFinished = true;
    }

    void LookingForPlayer()
    {
        if (_player)
        {
            _targetPosition = _player.transform.position;

            State = EnemyState.ChasingPlayer;
        }
        else
        {
            State = EnemyState.Observing;
        }
    }

    void ChasingPlayer()
    {
        if (_player && Vector2.Distance(transform.position, _player.transform.position) < 5f)
        {
            // update position more often if we are close enough
            LookingForPlayer();
        }
        else if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
        {
            // look for the new position
            State = EnemyState.LookingForPlayer;
        }
    }

    void AttackPlayer()
    {
        // find the player again
        State = EnemyState.LookingForPlayer;
    }

    void Observing()
    {
        // look around
        if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
        {
            // we are on the target position, pick new one
            _targetPosition = new Vector2(
                Random.Range(GameState.MinX, GameState.MaxX), 
                Random.Range(GameState.MinY, GameState.MaxY)
                );
        }
    }

    void OnPlayerDied()
    {
        State = EnemyState.Observing;
    }

    void OnLevelFinished()
    {
        State = EnemyState.Freezed;
        
        StartCoroutine(WaitAndDestroy());
    }

    public void TakeDamage()
    {
        lives--;

        if (lives <= 0)
        {
            int score = GameState.Level * killScorePoints;
            
            GameState.Score += score;
            SpawnScoreBalloon(score);
            
            DestroyEnemy();
        }
        else
        {
            int score = GameState.Level * hitScorePoints;

            GameState.Score += score;
            SpawnScoreBalloon(score);
        }
    }

    public void DestroyEnemy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        EventManager.TriggerEvent(Events.ENEMY_DIED);
        
        Destroy(gameObject);
    }
    
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        
        DestroyEnemy();
    }

    void SpawnScoreBalloon(int score)
    {
        ScoreBalloon scoreBalloon = Instantiate(scoreBalloonPrefab, transform.position, Quaternion.identity);
        scoreBalloon.SetScore(score);
    }
}

using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour, IDamage
{
    [SerializeField] float delayBetweenState = 1f;
    [SerializeField] private float chasingSpeed = 1f;
    [SerializeField] private float observingSpeed = 1f;

    [SerializeField] private GameObject explosion;

    [SerializeField] private int lives = 1;
    [SerializeField] private int hitScorePoints = 1;
    [SerializeField] private int killScorePoints = 1;

    [SerializeField] private int spawnHitCount = 1;
    [SerializeField] private Tool toolPrefab;

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
        switch (State)
        {
            case EnemyState.ChasingPlayer:
                Move(chasingSpeed);
                break;
            case EnemyState.Observing:
                Move(observingSpeed);
                break;
            default:
                _animator.SetBool("IsMoving", false);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player && State != EnemyState.Spawned)
        {
            State = EnemyState.AttackPlayer;
            
            EventManager.TriggerEvent(Events.PLAYER_UNDER_ATTACK);
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied); 
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }
    
    private void Move(float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _targetPosition,
            speed * Time.deltaTime
        );

        transform.localScale = new Vector2(transform.position.x < _targetPosition.x ? 1 : -1, 1);

        _animator.SetBool("IsMoving", true);
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

    void Spawned()
    {
        if (IsSpawningFinished)
            // once the spawning is finished we can go to next state
            State = EnemyState.LookingForPlayer;
        else
        {
            IsSpawningFinished = true;
        }
    }

    void LookingForPlayer()
    {
        // random state between chasing and observing
        if (Random.Range(0f, 100f) > 25f && _player)
        {
            _targetPosition = _player.transform.position;
            
            State = EnemyState.ChasingPlayer;
        }
        else
        {
            // we are on the target position, pick new random position
            _targetPosition = new Vector2(
                Random.Range(GameState.MinX, GameState.MaxX), 
                Random.Range(GameState.MinY, GameState.MaxY)
            );
            
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
            // look for the new position
            State = EnemyState.LookingForPlayer;
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

        SpawnToolIfRequired();

        State = EnemyState.ChasingPlayer;

        if (lives <= 0)
        {
            GameState.Score += GameState.Level * killScorePoints;
            DestroyEnemy();
        }
        else
            GameState.Score += GameState.Level * hitScorePoints;
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

    void SpawnToolIfRequired()
    {
        if (lives % spawnHitCount != 0)
            return;

        Instantiate(toolPrefab, transform.position, Quaternion.identity);
    }
}

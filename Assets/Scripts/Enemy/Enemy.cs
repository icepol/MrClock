using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamage
{
    [SerializeField] float delayBetweenState = 1f;
    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private GameObject explosion;

    [SerializeField] private int scorePoints = 1;
    
    public bool IsSpawningFinished { get; set; }

    private Vector2 _targetPosition;
    private Player _player;

    private enum State
    {
        Spawned,
        LookingForPlayer,
        ChasingPlayer,
        AttackPlayer,
        Observing,
        Freezed
    }

    private State state = State.Spawned;
    
    void Start()
    {
        _player = FindObjectOfType<Player>();

        StartCoroutine(NextState());
        
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }
    
    void Update()
    {
        if (state == State.ChasingPlayer || state == State.Observing)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _targetPosition, 
                moveSpeed * Time.deltaTime
                );
            
            transform.localScale = new Vector2(transform.position.x < _targetPosition.x ? 1 : -1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player && state != State.Spawned)
        {
            state = State.AttackPlayer;
            
            EventManager.TriggerEvent(Events.PLAYER_UNDER_ATTACK);
        }
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

            switch (state)
            {
                case State.Spawned:
                    Spawned();
                    break;
                case State.LookingForPlayer:
                    LookingForPlayer();
                    break;
                case State.ChasingPlayer:
                    ChasingPlayer();
                    break;
                case State.AttackPlayer:
                    AttackPlayer();
                    break;
                case State.Observing:
                    Observing();
                    break;
            }
        }
    }

    void Spawned()
    {
        if (IsSpawningFinished)
            // once the spawning is finished we can go to next state
            state = State.LookingForPlayer;
        else
        {
            IsSpawningFinished = true;
        }
    }

    void LookingForPlayer()
    {
        if (_player)
        {
            _targetPosition = _player.transform.position;

            state = State.ChasingPlayer;
        }
        else
        {
            state = State.Observing;
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
            state = State.LookingForPlayer;
        }
    }

    void AttackPlayer()
    {
        // find the player again
        state = State.LookingForPlayer;
    }

    void Observing()
    {
        // look around
        if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
        {
            // we are on the target position, pick new one
            _targetPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }
    }

    void OnPlayerDied()
    {
        state = State.Observing;
    }

    void OnLevelFinished()
    {
        state = State.Freezed;
        
    }

    public void TakeDamage()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        GameState.Score += GameState.Level * scorePoints;
        
        EventManager.TriggerEvent(Events.ENEMY_DIED);
        
        Destroy(gameObject);
    }
}

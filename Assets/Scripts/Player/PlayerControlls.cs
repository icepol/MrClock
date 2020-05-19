using System;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Player _player;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    
    private bool _isEnabled = true;
    private bool _isWalking = false;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();

        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
    }
    
    void Update()
    {
        if (_isEnabled)
            HandleMove();
        else
        {
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool("IsWalking", false);
        }
            
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
    }

    void HandleMove()
    {
        float xAxe = Input.GetAxisRaw("Horizontal");
        float yAxe = Input.GetAxisRaw("Vertical");

        Vector2 velocity = new Vector2(xAxe * moveSpeed, yAxe * moveSpeed);
        
        _rigidbody2D.velocity = velocity;

        bool isWalking = Mathf.Abs(_rigidbody2D.velocity.x) > 0 || Mathf.Abs(_rigidbody2D.velocity.y) > 0;

        _animator.SetBool("IsWalking", isWalking);

        if (isWalking && !_isWalking)
            StartWalking();
        else if (!isWalking && _isWalking)
            StopWalking();

        if (_rigidbody2D.velocity.x > 0.1f)
        {
            transform.localScale = new Vector2(1, 1);
            _player.FacingRight = true;
        }
        else if (_rigidbody2D.velocity.x < -0.1f)
        {
            transform.localScale = new Vector2(-1, 1);
            _player.FacingRight = false;
        }
    }
    
    void OnLevelFinished()
    {
        _isEnabled = false;
        
        StopWalking();
    }

    void OnPlayerDied()
    {
        _isEnabled = false;
        
        StopWalking();
    }

    void StartWalking()
    {
        _isWalking = true;
        _audioSource.Play();
    }

    void StopWalking()
    {
        _isWalking = false;
        _audioSource.Stop();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private Animator _animator;
    
    private bool _isLive = true;
    
    public bool FacingRight { get; set; }
    public PlayerInventory PlayerInventory => _playerInventory;

    private void Awake()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.AddListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
        EventManager.AddListener(Events.NET_DESTROYED, OnNetDestroyed);

        FacingRight = true;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
        EventManager.RemoveListener(Events.NET_DESTROYED, OnNetDestroyed);
    }

    private void OnPlayerUnderAttack()
    {
        Die();
    }

    private void OnNetDestroyed()
    {
        Die();
    }

    void Die()
    {
        if (!_isLive)
            return;

        _isLive = false;
        _animator.SetTrigger("Alarm");

        EventManager.TriggerEvent(Events.PLAYER_DIED);
        GameState.Lives -= 1;

        StartCoroutine(FinishDieing());
    }

    private IEnumerator FinishDieing()
    {
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
        
        EventManager.TriggerEvent(Events.LEVEL_FAILED);
    }
}

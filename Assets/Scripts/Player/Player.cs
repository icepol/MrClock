using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SoundEffects _soundEffects;
    private Animator _animator;
    private bool _isLive = true;
    
    public bool FacingRight { get; set; }
    
    private void Awake()
    {
        _soundEffects = GetComponent<SoundEffects>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.AddListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
    }

    private void OnPlayerUnderAttack()
    {
        if (!_isLive)
            return;

        _isLive = false;
        
        _soundEffects.PlayOnDie();
        
        EventManager.TriggerEvent(Events.PLAYER_DIED);
        
        _animator.SetTrigger("Alarm");

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

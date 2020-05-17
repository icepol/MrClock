using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
        EventManager.AddListener(Events.NET_BROKEN, OnNetDestroyed);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.ENEMY_DIED, OnEnemyDied);
        EventManager.RemoveListener(Events.NET_BROKEN, OnNetDestroyed);
    }

    void OnPlayerDied()
    {
        _animator.SetTrigger("ShakeBig");
    }

    void OnEnemyDied()
    {
        _animator.SetTrigger("ShakeSmall");
    }
    
    void OnNetDestroyed()
    {
        _animator.SetTrigger("ShakeBig");
    }
}

using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private float delayBetweenShooting = 0.2f;

    private Player _player;
    private float _delay;
    private bool _isEnabled = true;

    private void Awake()
    {
        _player = GetComponent<Player>();
        
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
    }
    
    void Update()
    {
        if (!_isEnabled)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space) && _delay <= 0)
            Fire();

        if (_delay > 0)
            _delay -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
    }

    void Fire()
    {
        _delay = delayBetweenShooting;

        Vector2 position = transform.position + new Vector3(0.1f, -0.2f, 0);
        
        Bullet bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.SetDirection(_player.FacingRight);
        
        EventManager.TriggerEvent(Events.PLAYER_SHOOT);
    }

    void OnPlayerDied()
    {
        _isEnabled = false;
    }

    void OnLevelFinished()
    {
        _isEnabled = false;
    }
}

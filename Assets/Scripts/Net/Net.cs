using System;
using System.Collections;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] private ExplosionSmall explosionPrefab;
    
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem netRepaired;

    [SerializeField] private int repairScorePoint = 5;
    
    private Animator _animator;

    public bool IsBroken { get; private set; }
    public bool IsRepaired { get; private set; }
    
    private bool _isDestroyed;

    private void Start()
    {
        fireParticle.Stop();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
            ContactWithPlayer(player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
            ContactWithEnemy(enemy);
    }
    
    public void Repair()
    {
        IsBroken = false;
        IsRepaired = true;
        
        _animator.SetBool("IsBroken", IsBroken);
        fireParticle.Stop();
        
        GameState.Score += GameState.Level * repairScorePoint;
        
        EventManager.TriggerEvent(Events.NET_REPAIRED);
        
        Instantiate(netRepaired, transform.position, Quaternion.identity, transform);
    }

    public void Broke()
    {
        IsBroken = true;
        _animator.SetBool("IsBroken", IsBroken);
        
        EventManager.TriggerEvent(Events.NET_BROKEN);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        fireParticle.Play();
    }
    
    void Destroy()
    {
        _isDestroyed = true;
        _animator.SetBool("IsDestroyed", _isDestroyed);
        
        EventManager.TriggerEvent(Events.NET_DESTROYED);
    }

    void ContactWithPlayer(Player player)
    {
        if (!IsBroken || _isDestroyed)
            return;
        
        if (player.PlayerInventory.ToolsCount > 0)
            Repair();
    }

    void ContactWithEnemy(Enemy enemy)
    {
        if (_isDestroyed)
            return;
        
        if (enemy.State != Enemy.EnemyState.ChasingPlayer)
            return;

        if (IsBroken)
            Destroy();
        else
            enemy.DestroyEnemy();
    }
}

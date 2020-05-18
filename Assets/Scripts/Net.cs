using System;
using System.Collections;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] private ExplosionSmall explosionPrefab;
    
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem netRepaired;
    
    private Animator _animator;

    public bool IsBroken { get; private set; }
    private bool _isDestroyed;

    private void Start()
    {
        fireParticle.Stop();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsBroken || _isDestroyed)
            return;
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            Destroy();
        }
    }

    public void Repair()
    {
        IsBroken = false;
        _animator.SetBool("IsBroken", IsBroken);
        fireParticle.Stop();
        
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
}

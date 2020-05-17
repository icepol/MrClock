using System;
using System.Collections;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] private ExplosionSmall explosionPrefab;
    [SerializeField] private ParticleSystem fireParticle;
    
    private Animator _animator;

    public bool IsBroken { get; private set; }

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
        if (player && IsBroken)
        {
            Repair();
        }
    }

    void Repair()
    {
        IsBroken = false;
        _animator.SetBool("IsBroken", IsBroken);
        fireParticle.Stop();
        
        EventManager.TriggerEvent(Events.NET_REPAIRED);
    }

    public void Broke()
    {
        IsBroken = true;
        _animator.SetBool("IsBroken", IsBroken);
        
        EventManager.TriggerEvent(Events.NET_BROKEN);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        fireParticle.Play();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float ttl = 1f;
    
    [SerializeField] private GameObject explosion;

    private bool _isRight;
    
    void Start()
    {
        Instantiate(explosion, transform.position, quaternion.identity);

        StartCoroutine(Timeout());
    }
    
    void Update()
    {
        transform.position += Vector3.right * (speed * Time.deltaTime * (_isRight ? 1 : -1));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.TakeDamage();

            Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(ttl);
        
        Destroy(gameObject);
    }

    public void SetDirection(bool isRight)
    {
        _isRight = isRight;
        
        transform.localScale = new Vector2(_isRight ? 1 : -1, 1);
    }
}

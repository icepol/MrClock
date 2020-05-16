using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSmall : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.2f;
    
    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}

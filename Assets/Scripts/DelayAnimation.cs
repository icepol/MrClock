using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimation : MonoBehaviour
{
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        StartCoroutine(WaitAndAnimate());
    }

    IEnumerator WaitAndAnimate()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));

        _animator.enabled = true;
    }
}

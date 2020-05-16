using System;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip underAttack;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void PlayOnJump()
    {
        if (jump)
            AudioSource.PlayClipAtPoint(jump, _cameraTransform.position);
    }

    public void PlayOnDie()
    {
        if (die)
            AudioSource.PlayClipAtPoint(die, _cameraTransform.position);
    }

    public void PlayUnderAttack()
    {
        if (underAttack)
            AudioSource.PlayClipAtPoint(underAttack, _cameraTransform.position);
    }
}

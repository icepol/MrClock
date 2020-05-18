using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        EventManager.AddListener(Events.TRANSITION_OPEN, OnTransitionOpen);
        EventManager.AddListener(Events.TRANSITION_CLOSE, OnTransitionClose);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TRANSITION_OPEN, OnTransitionOpen);
        EventManager.RemoveListener(Events.TRANSITION_CLOSE, OnTransitionClose);
    }

    private void OnTransitionOpen()
    {
        _animator.SetTrigger("Open");
    }

    private void OnTransitionClose()
    {
        _animator.SetTrigger("Close");
    }

    public void TransitionOpenFinished()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN_FINISHED);
    }

    public void TransitionCloseFinished()
    {
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE_FINISHED);
    }
}

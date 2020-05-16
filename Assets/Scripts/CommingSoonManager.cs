﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommingSoonManager : MonoBehaviour
{
    [SerializeField] private Text tryAgain;
    
    private void Awake()
    {
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    void Start()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    public void OnMenuButtonClick()
    {
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    public void OnTransitionCloseFinished()
    {
        SceneManager.LoadScene("Menu");
    }
}

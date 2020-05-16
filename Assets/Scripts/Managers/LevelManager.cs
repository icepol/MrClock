﻿using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool _isFinished;
    private bool _isMenuRequested;
    private bool _isGameOver;
        
    private void Awake()
    {
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
        EventManager.AddListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    void Start()
    {
        AnalyticsEvent.LevelStart(SceneManager.GetActiveScene().name);
        
        EventManager.TriggerEvent(Events.LEVEL_START);
    }
    
    void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isMenuRequested = true;
            EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
        }
    }

    void OnLevelStart()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);
    }
    
    void OnLevelFailed()
    {
        if (GameState.Lives <= 1)
        {
            _isGameOver = true;            
        }

        AnalyticsEvent.LevelFail(SceneManager.GetActiveScene().name);
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    void OnLevelFinished()
    {
        _isFinished = true;

        AnalyticsEvent.LevelComplete(SceneManager.GetActiveScene().name);
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    void OnTransitionCloseFinished()
    {
        if (_isFinished)
            LoadNextLevel();
        else if (_isMenuRequested)
            ShowMenu();
        else if (_isGameOver)
            ShowGameOver();
        else
            Restart();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ShowMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    void ShowGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    
    void LoadNextLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex + 1);
    }
}

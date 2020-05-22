using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

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
        EventManager.AddListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.AddListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
    }

    void Start()
    {
        GameAnalytics.Initialize();
        
        EventManager.TriggerEvent(Events.LEVEL_START);
    }
    
    void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
        EventManager.RemoveListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.RemoveListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
    }

    void OnBoundariesBottomLeft(Vector3 vector)
    {
        GameState.MinX = vector.x;
        GameState.MinY = vector.y;
    }

    void OnBoundariesTopRight(Vector3 vector)
    {
        GameState.MaxX = vector.x;
        GameState.MaxY = vector.y;
    }

    void OnLevelStart()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, SceneManager.GetActiveScene().name);
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);
    }
    
    void OnLevelFailed()
    {
        if (GameState.Lives <= 0)
            _isGameOver = true;
        
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, SceneManager.GetActiveScene().name);
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    void OnLevelFinished()
    {
        _isFinished = true;
        
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, SceneManager.GetActiveScene().name);

        StartCoroutine(WaitAndClose());
    }

    IEnumerator WaitAndClose()
    {
        yield return new WaitForSeconds(2f);
        
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

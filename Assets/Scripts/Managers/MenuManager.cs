using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private bool _settings;
    private bool _quit;
    private bool _keyPressed;
    
    private void Awake()
    {
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    void Start()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);
        
        # if UNITY_WEBGL
            playButton.transform.position = settingsButton.transform.position;
            settingsButton.transform.position = quitButton.transform.position;
        
            quitButton.gameObject.SetActive(false);
        #endif
    }

    private void Update()
    {
        if (!_keyPressed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _keyPressed = true;
                EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _keyPressed = true;
                _quit = true;
                EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
            }
        } 
    }
    
    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    public void OnPlayButtonClick()
    {
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }
    
    public void OnSettingsButtonClick()
    {
        _settings = true;
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    public void OnQuitButtonClick()
    {
        _quit = true;
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    public void OnTransitionCloseFinished()
    {
        if (_quit)
            Application.Quit();
        else if (_settings)
        {
            SceneManager.LoadScene("Settings");
        }
        else
        {
            GameState.Reset();
            SceneManager.LoadScene("Scenes/Levels/1");
        }
    }
}

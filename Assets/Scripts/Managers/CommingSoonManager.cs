using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommingSoonManager : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private GameObject webGLPanel;
    [SerializeField] private GameObject restOSPanel;
    
    private void Awake()
    {
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    void Start()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);

        score.text = GameState.Score.ToString();

#if UNITY_WEBGL
        webGLPanel.SetActive(true);
        restOSPanel.SetActive(false);
#else
        webGLPanel.SetActive(false);
        restOSPanel.SetActive(true);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }
    
    void OnTransitionCloseFinished()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnMenuButtonClick()
    {
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    public void OnWantMoreLinkClick()
    {
        Application.OpenURL("https://pixelook.itch.io/mrclock");
    } 
}

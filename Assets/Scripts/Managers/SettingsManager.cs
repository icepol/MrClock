using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    
    private bool _keyPressed;
    
    private void Awake()
    {
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    void Start()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);

        UpdateUI();
    }

    private void Update()
    {
        if (!_keyPressed && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)))
        {
            _keyPressed = true;
            EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }
    
    public void OnSfxButtonClick()
    {
        Settings.IsSfxEnabled = !Settings.IsSfxEnabled;
        
        UpdateUI();
    }
    
    public void OnMusicButtonClick()
    {
        Settings.IsMusicEnabled = !Settings.IsMusicEnabled;
        
        UpdateUI();
        
        EventManager.TriggerEvent(Events.MUSIC_SETTINGS_CHANGED);
    }
    
    public void OnMenuButtonClick()
    {
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    public void OnTransitionCloseFinished()
    {
        SceneManager.LoadScene("Menu");
    }

    private void UpdateUI()
    {
        sfxButton.GetComponentInChildren<Text>().text = Settings.IsSfxEnabled ? "ON" : "OFF";
        musicButton.GetComponentInChildren<Text>().text = Settings.IsMusicEnabled ? "ON" : "OFF";
    }
}

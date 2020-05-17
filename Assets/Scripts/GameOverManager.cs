using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Text score;
    
    private bool _keyPressed;
    
    private void Awake()
    {
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
    }

    void Start()
    {
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);

        score.text = GameState.Score.ToString();
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
    
    public void OnMenuButtonClick()
    {
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    public void OnTransitionCloseFinished()
    {
        SceneManager.LoadScene("Menu");
    }
}

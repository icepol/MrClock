using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Text lives;
    [SerializeField] private Text score;
    [SerializeField] private Text tools;
    
    void Update()
    {
        lives.text = "x" + GameState.Lives;
        tools.text = "x" + GameState.Tools;
        
        score.text = GameState.Score.ToString();
    }
}

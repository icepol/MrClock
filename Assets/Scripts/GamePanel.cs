using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Text lives;
    [SerializeField] private Text score;
    
    void Update()
    {
        lives.text = "x" + GameState.Lives;
        score.text = GameState.Score.ToString();
    }
}

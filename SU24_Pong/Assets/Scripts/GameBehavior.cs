using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    // `static` ensures that there is only of something

    public enum StateMachine
    {
        Play,   // 0
        Pause   // 1
    }

    public StateMachine State;


    [SerializeField] private TextMeshProUGUI _pauseGUI;
    
    public Player[] Players = new Player[2];

    public float PaddleSpeed = 7.0f;
    public float InitialBallSpeed = 6.0f;    
    public float BallSpeedIncrement = 1.25f;
    
    [SerializeField] int ScoreGoal = 5;
    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)   // if exist
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetGame();
    }

    void ResetGame()
    {
        foreach (Player p in Players)       // temporary placeholder
        {
            p.Score = 0;
        }

        State = StateMachine.Play;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            State = State == StateMachine.Play ? StateMachine.Pause : StateMachine.Play;
            _pauseGUI.enabled = !_pauseGUI.enabled;     // flipping the boolean
        }
    }
    

    public float CalculateYLimit(GameObject gO)
    {
        SpriteRenderer renderer = gO.GetComponent<SpriteRenderer>();
        float spriteHeight = renderer.bounds.size.y;
        return Utilities.CalculateYLimit(spriteHeight);
    }

    public void ScorePoint(int player) {
        Players[player].Score++;
        CheckWinner();
    }
    
    void CheckWinner() {
        foreach(Player player in Players) {
            if (player.Score >= ScoreGoal) {
                ResetGame();
            }
        }
    }
}

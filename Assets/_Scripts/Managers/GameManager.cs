using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[Serializable]
public enum GameState
{
    Starting,
    Running,
    Pause
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InputReader input;

    [field:SerializeField] public GameState State { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        input.PauseEvent += PauseGame;
        input.ResumeEvent += ResumeGame;

        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                break;
            case GameState.Running:
                break;
            case GameState.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnGameStateChanged?.Invoke(State);
    }

    public void PauseGame()
    {
        ChangeState(GameState.Pause);
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Running);
    }
}
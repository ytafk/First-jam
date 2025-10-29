////using System.Collections;
//using System.Collections.Generic;
//using System;
//using UnityEngine;

////public enum GameState { Placement, Running, Win, Lose }

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;
//    public RocketController rocket;
//    public event Action<GameState> OnStateChanged;

//    public GameState State { get; private set; }

//    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

//    void Start() { SetState(GameState.Placement); }

//    public void SetState(GameState s)
//    {
//        State = s;
//        switch (s)
//        {
//            case GameState.Placement: rocket.Stop(); break;
//            case GameState.Running: rocket.Run(); break;
//            case GameState.Win:
//            case GameState.Lose: rocket.Stop(); break;
//        }
//        OnStateChanged?.Invoke(State);
//    }

//    public void StartGame() { if (State == GameState.Placement) SetState(GameState.Running); }
//    public void WinGame() { if (State == GameState.Running) SetState(GameState.Win); }
//    public void LoseGame() { if (State == GameState.Running) SetState(GameState.Lose); }
//}

using System;
using UnityEngine;

public enum GameState { Placement, Running, Win, Lose }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public RocketController rocket; // Roket scripti
    public event Action<GameState> OnStateChanged;

    // GameManager (örnek)
   

    public GameState State { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetState(GameState.Placement);
    }

    public void SetState(GameState s)
    {
        State = s;

        switch (s)
        {
            case GameState.Placement:
                rocket?.Stop();
                break;
            case GameState.Running:
                rocket?.Run();
                break;
            case GameState.Win:
            case GameState.Lose:
                rocket?.Stop();
                break;
        }

        OnStateChanged?.Invoke(State);
    }

    public void StartGame()
    {
        if (State == GameState.Placement)
            SetState(GameState.Running);
    }

    public void WinGame()
    {
        if (State == GameState.Running)
            SetState(GameState.Win);
    }

    public void LoseGame()
    {
        if (State == GameState.Running)
            SetState(GameState.Lose);
    }
}



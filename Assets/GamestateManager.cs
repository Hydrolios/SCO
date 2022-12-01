using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamestateManager
{
    private static GamestateManager _instance;
    public static GamestateManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GamestateManager();
            return _instance;
        }
    }

    public Gamestate CurrentGamestate { get; private set; }

    public delegate void GamestateChangeHandler(Gamestate newGamestate);
    public event GamestateChangeHandler OnGamestateChanged;
    private GamestateManager()
    {

    }

    public void SetState (Gamestate newGamestate)
    {
        if (newGamestate == CurrentGamestate)
            return;

        CurrentGamestate = newGamestate;
        OnGamestateChanged?.Invoke(newGamestate);
    }

}

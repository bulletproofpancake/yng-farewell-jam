using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public UnityEvent gameStart, gameEnd;
    public event Action GameStart, GameEnd;
    public List<int> downedPlayers;
    public bool isEndGame;

    public void OnGameStart()
    {
        gameStart?.Invoke();
        GameStart?.Invoke();
    }

    public void OnGameEnd()
    {
        print("OnGameEnd");
        if(downedPlayers.Count == 3)
        {
            isEndGame = true;
            gameEnd?.Invoke();
            GameEnd?.Invoke();
        }
    }

    public void Exit() => Application.Quit();
}

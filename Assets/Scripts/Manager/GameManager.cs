using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent gameStart, gameEnd;
    public event Action GameStart, GameEnd;

    public void OnGameStart()
    {
        gameStart?.Invoke();
        GameStart?.Invoke();
    }

    public void OnGameEnd()
    {
        gameEnd?.Invoke();
        GameEnd?.Invoke();
    }

    public void Exit() => Application.Quit();
}

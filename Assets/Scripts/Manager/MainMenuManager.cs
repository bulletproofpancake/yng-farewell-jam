using System;
using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.GameStart += OnGameStart;
    }
    private void OnDisable()
    {
        gameManager.GameStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        transform.DOLocalMoveY(600, 1f);
    }
}

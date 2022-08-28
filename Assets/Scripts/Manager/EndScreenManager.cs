using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerRankDisplay;
    private GameManager _gameManager;
    private PlayerInputManager playerInputManager;
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerRankDisplay.text = string.Empty;
        _gameManager.downedPlayers.Reverse();
        
        for (int i = 0; i < playerInputManager.maxPlayerCount; i++)
        {
            if (!_gameManager.downedPlayers.Contains(i))
            {
                playerRankDisplay.text += $"{1}. Player {i+1}\n";
            }
        }
        
        for(int i = 0; i < _gameManager.downedPlayers.Count; i++)
        {
            playerRankDisplay.text += $"{i + 2}. Player {_gameManager.downedPlayers[i]+1}\n";
        }
        transform.DOLocalMoveY(0, 1f);
    }
}

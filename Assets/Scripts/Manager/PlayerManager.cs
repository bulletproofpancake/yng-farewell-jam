using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints = new List<Transform>();
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    void Start()
    {
        for (int i = 0; i < playerInputManager.maxPlayerCount; i++)
        {
            PlayerInput.Instantiate(playerPrefab, i, controlScheme: $"Keyboard {i + 1}", pairWithDevice: Keyboard.current);
        }
    }

    void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }
    void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        player.transform.position = startingPoints[players.Count - 1].position;
    }
}

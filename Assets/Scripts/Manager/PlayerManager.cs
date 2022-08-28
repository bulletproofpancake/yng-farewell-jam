using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    public List<Transform> startingPoints = new List<Transform>();
    public List<HealthManager> healthManagers = new List<HealthManager>();
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    void Start()
    {
        for (int i = 0; i < playerInputManager.maxPlayerCount; i++)
        {
           var player = PlayerInput.Instantiate(playerPrefab, i, controlScheme: $"Keyboard {i + 1}", pairWithDevice: Keyboard.current);
           player.GetComponent<PlayerController>().healthManager = healthManagers[player.playerIndex];
        }
    }

    void OnEnable()
    {
        playerInputManager.onPlayerJoined += SpawnPlayer;
    }
    
    void OnDisable()
    {
        playerInputManager.onPlayerJoined -= SpawnPlayer;
    }

    public void SpawnPlayer(PlayerInput player)
    {
        player.transform.position = startingPoints[player.playerIndex].position;
    }
}

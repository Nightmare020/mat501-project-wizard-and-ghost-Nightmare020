
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayersConnectionManager : MonoBehaviour
{
    //private List<PlayerInput> players;
    [SerializeField] private Transform startingPointWizard;
    [SerializeField] private Transform startingPointGhost;
    [SerializeField] private GameObject playerPrefab;
    private PlayerInputManager _playerInputManager;
    private LoadWizardGhost _selectionGhostPanel;


    private void Awake()
    {
        //players = new List<PlayerInput>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _selectionGhostPanel = FindObjectOfType<LoadWizardGhost>();
    }

    private void Start()
    {
        if (playerPrefab != null)
        {
            // Instantiate and configure wizard
            GameObject wizardObject = Instantiate(playerPrefab, startingPointWizard.position, Quaternion.identity);
            PlayerManager wizardManager = wizardObject.GetComponent<PlayerManager>();
            wizardManager.SetCurrentState(PlayerState.WizardAI);

            // Instantiate and configure ghost
            GameObject ghostObject = Instantiate(playerPrefab, startingPointGhost.position, Quaternion.identity);
            PlayerManager ghostManager = ghostObject.GetComponent<PlayerManager>();
            ghostManager.SetCurrentState(PlayerState.GhostAI);

            Debug.Log("Both Wizard and Ghost have been initialized.");
        }
        else
        {
            Debug.LogError("Player Prefab is not assigned.");
        }
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
        //_playerInputManager.onPlayerLeft += PlayerLeft;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
        //_playerInputManager.onPlayerLeft -= PlayerLeft;
    }


    private void AddPlayer(PlayerInput player)
    {
        //players.Add(player);
        //player.transform.position = (Vector2)startingPoints[players.Count - 1].position;

        // Set the player's position to the starting point
        if (startingPointWizard != null)
        {
            player.transform.position = startingPointWizard.position;
        }
        else 
        {
            Debug.LogWarning("Starting point not assigned");
        }

        //set on the selection panel the number of players conected
        MyInputManager aux = player.GetComponent<MyInputManager>();
        _selectionGhostPanel.AddPlayer(aux);
    }

    //private void PlayerLeft(PlayerInput player)
    //{
    //    foreach (PlayerInput p in players)
    //    {
    //        MyInputManager aux = p.transform.gameObject.GetComponent<MyInputManager>();
    //        aux.SetInputMap(CurrentInputState.UiNavigation);


    //        //remove player from selection manager
    //        if (p == player)
    //        {
    //            _selectionWizardGhostPanel.RemovePlayer(aux);
    //        }
    //    }
    //    players.Remove(player);
    //}
}
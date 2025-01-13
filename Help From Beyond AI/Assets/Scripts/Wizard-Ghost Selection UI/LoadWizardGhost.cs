using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadWizardGhost : MonoBehaviour
{
    // Start is called before the first frame update
    private MyInputManager playerInputManager;
    private PlayerManager playerManager;
    private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private Image playerImage;
    //[SerializeField] private Transform wizardX, ghostX;

    //inputs
    public bool inputEnabled;

    //accept image
    [SerializeField] private CanvasGroup acceptImageCanvas;
    //private float originalImageX;

    private void Awake()
    {
        //players = new List<MyInputManager>();
        //playerRolPosition = new List<int>();
        //playerManagers = new List<PlayerManager>();
        _canvasGroup = GetComponent<CanvasGroup>();
        //originalImageX = playerImages[0].transform.position.x;
        //UpdateAcceptImage(false);
    }

    public void ShowUI()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        inputEnabled = true;
        //UpdateAcceptImage(false);
    }

    public void HideUI()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        inputEnabled = false;
    }

    public void AddPlayer(MyInputManager inputManager)
    {
        //players.Add(playerInputManager);
        //int index = players.Count - 1;
        //if (players.Count == 2)
        //    playerRolPosition.Add(playerRolPosition[0]);
        //else
        //    playerRolPosition.Add(0);

        //playerManagers.Add(playerInputManager.transform.GetComponent<PlayerManager>());

        //playerInputManager = inputManager;
        //playerManager = inputManager.transform.GetComponent<PlayerManager>();

        //playerImage.color = Color.green;
        //startText.text = "START TRAINING!";

        //update the player count
        //SetPlayerCountText();
        //UpdateAcceptImage();
    }

    public void RemovePlayer()
    {
        //    int deletedPlayerIndex = players.FindIndex(x => x == myInputManager);
        //    players.RemoveAt(deletedPlayerIndex);
        //    playerManagers.RemoveAt(deletedPlayerIndex);
        //    playerRolPosition.RemoveAt(deletedPlayerIndex);
        //    playerImages[players.Count].color = Color.white;
        //    foreach (var image in playerImages)
        //    {
        //        Vector2 newPos = new Vector2(originalImageX, image.transform.position.y);
        //        image.transform.position = newPos;
        //    }

        //    for (int i = 0; i < playerRolPosition.Count; i++)
        //    {
        //        playerRolPosition[i] = 0;
        //    }

        //playerInputManager = null;
        //playerManager = null;
        //playerImage.color = Color.white;

        //    //update the player count
        //    ShowUI();
        //    SetPlayerCountText();
        //    UpdateAcceptImage();
    }

    //private void SetPlayerCountText()
    //{
    //    playerCountText.text = countPlayer + $"/1";
    //}

    #region player inputs

    //public void PlayerAccept()
    //{
    //    // Ensure a player is connected
    //    if (inputEnabled)
    //    {
    //        // Set players as ghost
    //        //playerInputManager.SetInputMap(CurrentInputState.Wizard);
    //        //playerManager.SetCurrentState(PlayerState.Wizard);

    //        // Hide menu and disable the inputs
    //        HideUI();
    //        print("Game Started");

    //        //SetWizardOrGhost(0);
    //        //SetWizardOrGhost(1);
    //    }
    //}

    //public void SelectLeft(MyInputManager myInputManager)
    //{
    //    int playerIndex = players.FindIndex(x => x == myInputManager);
    //    playerRolPosition[playerIndex] = 1;
    //    Vector2 newPos = new Vector2(wizardX.position.x, playerImages[playerIndex].transform.position.y);
    //    playerImages[playerIndex].transform.position = newPos;
    //    UpdateAcceptImage();
    //}

    //public void SelectRight(MyInputManager myInputManager)
    //{
    //    int playerIndex = players.FindIndex(x => x == myInputManager);
    //    playerRolPosition[playerIndex] = -1;
    //    Vector2 newPos = new Vector2(ghostX.position.x, playerImages[playerIndex].transform.position.y);
    //    playerImages[playerIndex].transform.position = newPos;
    //    UpdateAcceptImage();
    //}

    #endregion


    //public void UpdateAcceptImage(bool show)
    //{
    //    if (show)
    //    {
    //        playerImage.color = Color.green;
    //        startText.text = "START TRAINING!";
    //        acceptImageCanvas.alpha = 1;
    //    }
    //    else
    //    {
    //        playerImage.color = Color.white;
    //        acceptImageCanvas.alpha = 0;
    //    }

    //    if (players.Count == 2 && inputEnabled && playerRolPosition[0] * playerRolPosition[1] == -1)
    //    {
    //        ShowAcceptText();
    //    }
    //    else
    //    {
    //        HideAcceptText();
    //    }
    //}

    //public void ShowAcceptText()
    //{
    //    acceptImageCanvas.alpha = 1;
    //}

    //public void HideAcceptText()
    //{
    //    acceptImageCanvas.alpha = 0;
    //}

    //private void SetWizardOrGhost(int index)
    //{
    //    if (playerRolPosition[index] == 1)
    //    {
    //        players[index].SetInputMap(CurrentInputState.Wizard);
    //        playerManagers[index].SetCurrentState(PlayerState.Wizard);
    //    }
    //    else if (playerRolPosition[index] == -1)
    //    {
    //        players[index].SetInputMap(CurrentInputState.Ghost);
    //        playerManagers[index].SetCurrentState(PlayerState.Ghost);
    //    }
    //    else
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEditorInternal;

public class Menu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject createRoomScreen;
    public GameObject lobbyScreen;
    public GameObject lobbyBrowserScreen;
    [Header("Main Screen")]
    public Button createRoom;
    public Button findRoom;
    [Header("Lobby")]
    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI roomInfoText;
    public Button startGameButton;
    [Header("Lobby Browser")]
    public RectTransform roomListConteiner;
    public GameObject roomButtonPrefab;

    private List<GameObject> roomButtons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();
    private void Start()
    {
        //disable room buttons at start
        createRoom.interactable = false;
        findRoom.interactable = false;
        Cursor.lockState = CursorLockMode.None;
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }
    //change currenttly visable screen
    void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        lobbyBrowserScreen.SetActive(false);
        screen.SetActive(true);// set on requested screen
    }
    //called when back button is pressed
    public void OnBackButton()
    {
        SetScreen(mainScreen);
    }
    public void OnPlayerValueChange(TMP_InputField inputName)
    {
        PhotonNetwork.NickName = inputName.text;
    }
    public override void OnConnectedToMaster()
    {
        // eneable menu buttons when connected to the serever
        createRoom.interactable = true;
        findRoom.interactable = true;
    }
    public void OnCreateRoonButton()
    {
        SetScreen(createRoomScreen);
    }
    public void OnFindRoomButton()
    {
        SetScreen(lobbyBrowserScreen);
    }
    // CREATE ROOM SCREAN
    public void OnCreateButton(TMP_InputField roomNameInput)
    {
        NetworkManager.manager.CreateRoom(roomNameInput.text);
    }
    // LOBBY SCREEN
    public override void OnJoinedLobby()
    {
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // when player left update lobby
        UpdateLobbyUI();
    }
    [PunRPC]
    public void UpdateLobbyUI()
    {
        // disable or enable start button depending on if we'are host
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
        // display all the players
        playerListText.text = "";
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName+"/n";
        }
        roomInfoText.text = "<b>RoomName</b>" + PhotonNetwork.CurrentRoom.Name;
    }
    public void OnStartGameButton()
    {
        //hide the room
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // tell everyone change screen game begin
        NetworkManager.manager.photonView.RPC("ChangeScene", RpcTarget.All,"Game");
    }
    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
 * Allows connecting to the server singleton patern
 */
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;
    // instance 
    public static NetworkManager manager;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start()
    {
        // connect to the master server
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connected");
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Joined Lobby");
    }
    public void CreateRoom(string _roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)this.maxPlayers;
        PhotonNetwork.CreateRoom(_roomName, options);
    }
    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
        Debug.Log("Joined Room"+_roomName);
    }
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        //PhotonNetwork.LoadLevel("Menu");
    }
}

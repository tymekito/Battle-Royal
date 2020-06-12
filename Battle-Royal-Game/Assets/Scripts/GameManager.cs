using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabLocation;
    public PlayerController[] player;
    public Transform[] spawnPoints;
    public int alivePlayer;

    private int playerInGame;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = new PlayerController[PhotonNetwork.PlayerList.Length];
        alivePlayer = player.Length;
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void ImInGame()
    {
        playerInGame++;
        if(PhotonNetwork.IsMasterClient && playerInGame==PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        player.GetComponent<PlayerController>().photonView.RPC("Initialize", RpcTarget.AllBuffered,PhotonNetwork.LocalPlayer);
        //init players

    }
}

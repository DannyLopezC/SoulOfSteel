using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;

public class PhotonGame : MonoBehaviourPunCallbacks {
    private Photon.Realtime.Player[] _players;
    private int _playerNumber;
    private GameObject _playerGameObject;

    public PhotonView pv;

    private const string PLAYER_PREFAB_PATH = "Player";
    private const string MAIN_MENU_SCENE = "MainMenu";

    private void Start() {
        pv = GetComponent<PhotonView>();

        UIManager.Instance.ShowWaitingForOpponentPanel();
    }

    public override void OnJoinedRoom() {
        SpawnPlayer();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        if (PhotonNetwork.PlayerList.Length == 2) {
            UIManager.Instance.ShowGamePanel();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
        Debug.Log($"A player left the room");
        PhotonNetwork.LoadLevel(MAIN_MENU_SCENE);
        PhotonNetwork.LeaveRoom();
    }

    private void SpawnPlayer() {
        _players = PhotonNetwork.PlayerList;
        _playerNumber = _players.Length;

        PhotonNetwork.NickName = _playerNumber.ToString();

        _playerGameObject = PhotonNetwork.Instantiate(PLAYER_PREFAB_PATH, transform.position, Quaternion.identity, 0);

        _playerGameObject.TryGetComponent(out PlayerView currentPlayer);
        GameManager.Instance.playerList.Add(currentPlayer);

        if (PhotonNetwork.PlayerList.Length == 2) {
            UIManager.Instance.ShowGamePanel();
        }
    }
}
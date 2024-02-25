using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks {
    void Start() {
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log($"It is going to connect to the master server");
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        
        Debug.Log($"It has been connected to the master server");
        GameManager.Instance.OnMasterServerConnected.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log($"Disconnected from the server");
    }
}

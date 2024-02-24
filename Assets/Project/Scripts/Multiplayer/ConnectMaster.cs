using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectMaster : MonoBehaviourPunCallbacks {
    private static ConnectMaster _instance;

    public static ConnectMaster Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ConnectMaster>();

                if (_instance == null) {
                    GameObject singletonObject = new GameObject("Master server");
                    _instance = singletonObject.AddComponent<ConnectMaster>();
                }
            }
        
            return _instance;
        }
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log($"It is going to connect to the master server");
    }

    public override void OnConnectedToMaster() {
        Debug.Log($"It has been connected to the master server");
        
        GameManager.Instance.OnMasterServerConnected.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log($"Disconnected from the server");
    }
}

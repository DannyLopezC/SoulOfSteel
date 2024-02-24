using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks {

    private List<int> roomIds;

    void Start() {
        roomIds = new List<int>();
    }

    // joining
    
    public void JoinRoom() {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log($"Joining to a random room");
    }

    public override void OnJoinedRoom() {
        Debug.Log($"Joined to the room");
        
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log($"Failed joining to a random room, creating a new one");
        
        CreateRoom();
    }
    
    // creating

    private void CreateRoom() {
        int roomId = Random.Range(1, 100);
        roomIds.Add(roomId);

        Debug.Log($"Creating a new room");
        PhotonNetwork.JoinOrCreateRoom($"Room #{roomId}", new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnCreatedRoom() {
        Debug.Log($"Room created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log($"Failed to create a new room, trying again");
        
        CreateRoom();
    }
}

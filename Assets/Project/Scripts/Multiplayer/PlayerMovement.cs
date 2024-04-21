using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public PhotonView pv;

    private void Start() {
        pv = GetComponent<PhotonView>();
        GameManager.Instance.OnMovementSelectedEvent += DoMove;
        
    }

    private void DoMove(Movement movement) {
        
    }

    private void MoveToCell(Vector2 index) {
        if (!GameManager.Instance.LocalPlayerInstance.PlayerController.GetMoving()) return;

        if (pv.IsMine) {
            transform.position = GameManager.Instance.boardView.GetCellPos(index);
        }
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) GameManager.Instance.OnMovementSelectedEvent -= DoMove;
    }
}
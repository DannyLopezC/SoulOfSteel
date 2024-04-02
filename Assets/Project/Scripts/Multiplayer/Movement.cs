using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float velocity = 0.1f;

    public PhotonView pv;

    private BoardView board;

    private void Start() {
        pv = GetComponent<PhotonView>();
        GameManager.Instance.OnCellClickedEvent += OnMovement;
    }

    private void Update() {
    }

    private void OnMovement(Vector2 index) {
        // if (pv.IsMine) {
        Debug.Log($"{index}");
        transform.position = GameManager.Instance.boardView.GetCellPos(index);
        // }
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) GameManager.Instance.OnCellClickedEvent -= OnMovement;
    }
}

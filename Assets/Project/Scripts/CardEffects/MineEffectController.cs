﻿using System.Collections.Generic;
using UnityEngine;

public interface IMineEffectController : IEffectController {
}

public class MineEffectController : EffectController, IMineEffectController {
    public List<Vector2> MinedCells { private set; get; }
    private readonly int _minesAmount;

    public MineEffectController(int minesAmount) {
        MinedCells = new List<Vector2>();
        _minesAmount = minesAmount;
    }

    public override void Activate(int originId) {
        EffectManager.Instance.OnCellsSelectedEvent += SetMines;
        GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() == originId)
            .SelectCells(_minesAmount);
    }

    private void SetMines(List<Vector2> cellsSelected) {
        BoardView board = GameManager.Instance.boardView;

        foreach (Vector2 index in cellsSelected) {
            GameManager.Instance.boardView.SetBoardStatusCellType(index, CellType.Mined);
        }

        Debug.Log($"mines put");
        EffectManager.Instance.OnCellsSelectedEvent -= SetMines;
    }
}
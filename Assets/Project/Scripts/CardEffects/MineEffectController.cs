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
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetDoingEffect(true);

        EffectManager.Instance.OnCellsSelectedEvent += StopSettingMines;
        EffectManager.Instance.OnSelectedCellEvent += SetMines;
        GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() == originId)
            .SelectCells(_minesAmount);
    }

    private void SetMines(Vector2 index, bool select) {
        GameManager.Instance.boardView.SetBoardStatusCellType(index, select ? CellType.Mined : CellType.Normal);
    }

    private void StopSettingMines(List<Vector2> cellsSelected) {
        // Debug.Log($"mines put");
        GameManager.Instance.OnAllEffectsFinished();
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetDoingEffect(false);
        EffectManager.Instance.OnSelectedCellEvent -= SetMines;
        EffectManager.Instance.OnCellsSelectedEvent -= StopSettingMines;
    }
}
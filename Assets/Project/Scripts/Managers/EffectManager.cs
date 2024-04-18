using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviourSingleton<EffectManager> {
    private MineEffectController _mineEffectController;

    public event Action<Vector2> OnSelectedCellEvent; //when selecting a single cell
    public event Action<List<Vector2>> OnCellsSelectedEvent; //when all cells have been selected

    public void PutMines(int originId, int amount) {
        _mineEffectController = new MineEffectController(amount);
        _mineEffectController.Activate(originId);
    }

    public void CellsSelected(List<Vector2> cellsSelected) {
        OnCellsSelectedEvent?.Invoke(cellsSelected);
    }

    public void CellSelected(Vector2 index) {
        OnSelectedCellEvent?.Invoke(index);
    }
}
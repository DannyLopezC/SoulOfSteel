using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CellType {
    Normal,
    Mined
}

public interface ICellController {
    void SetType(CellType type);
    CellType GetCellType();
}

public class CellController : ICellController {
    private readonly ICellView _view;
    public CellType CellType { private set; get; }

    public CellController(ICellView view, CellType cellType) {
        _view = view;
        CellType = cellType;
    }

    public void SetType(CellType type) {
        CellType = type;
    }

    public CellType GetCellType() {
        return CellType;
    }
}
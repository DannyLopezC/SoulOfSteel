using System;
using Unity.VisualScripting;
using UnityEngine;

public interface ICellView {
}

public class CellView : MonoBehaviour, ICellView {
    public float cellXSize;
    public float cellYSize;

    private ICellController _cellController;

    public ICellController CellController {
        get { return _cellController ??= new CellController(this, CellType.Normal); }
    }

    private void Start() {
    }

    public void SetSize() {
        TryGetComponent(out RectTransform recTransform);

        recTransform.sizeDelta = new Vector2(cellXSize, cellYSize);
    }
}
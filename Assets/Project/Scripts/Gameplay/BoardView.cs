using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public interface IBoardView {
    Transform GetTransform();
    float GetXBoardSize();
    float GetYBoardSize();
    GameObject InstantiateCellView();
    List<List<CellView>> GetBoardStatus();
    void SetBoardStatus(List<List<CellView>> board);
}

public class BoardView : MonoBehaviour, IBoardView {
    [OnValueChanged("GenerateBoard")] public float xBoardSize;
    [OnValueChanged("GenerateBoard")] public float yBoardSize;
    [OnValueChanged("GenerateBoard")] public float offset;
    public GameObject cellPrefab;

    [SerializeField] public List<List<CellView>> _boardStatus;

    private IBoardController _boardController;

    public IBoardController BoardController {
        get { return _boardController ??= new BoardController(this); }
    }

    private void Awake() {
        // GameManager.Instance.boardView = this;
    }

    private void Start() {
        Debug.Log($"{_boardStatus}");
        GenerateBoard();
    }

    public Transform GetTransform() {
        return transform;
    }

    public float GetXBoardSize() {
        return xBoardSize;
    }

    public float GetYBoardSize() {
        return xBoardSize;
    }

    [Button]
    public void GenerateBoard() {
        _boardStatus = new List<List<CellView>>();
        DestroyTransformChildren();

        BoardController.GenerateBoardCells(xBoardSize / BoardController.GetBoardCount(),
            yBoardSize / BoardController.GetBoardCount(), offset, cellPrefab);

        for (int i = 0; i < _boardStatus.Count; i++) {
            for (int j = 0; j < _boardStatus[i].Count; j++) {
                Debug.Log($"{_boardStatus[j][i].index}");
            }
        }
    }

    private void DestroyTransformChildren() {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public GameObject InstantiateCellView() {
        return Instantiate(cellPrefab);
    }

    public List<List<CellView>> GetBoardStatus() {
        return _boardStatus;
    }

    public Vector3 GetCellPos(Vector2 index) {
        Debug.Log($"{index}");
        return _boardStatus[(int)index.y][(int)index.x].transform.position;
    }

    public void SetBoardStatus(List<List<CellView>> board) {
        _boardStatus = board;
    }
}

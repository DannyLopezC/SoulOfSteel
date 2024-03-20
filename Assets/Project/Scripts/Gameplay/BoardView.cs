using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public interface IBoardView {
    Transform GetTransform();
    float GetXBoardSize();
    float GetYBoardSize();
    GameObject InstantiateCellView();
}

public class BoardView : MonoBehaviour, IBoardView {
    [OnValueChanged("GenerateBoard")] public float xBoardSize;
    [OnValueChanged("GenerateBoard")] public float yBoardSize;
    public GameObject cellPrefab;
    
    private IBoardController _boardController;

    public IBoardController BoardController {
        get { return _boardController ??= new BoardController(this); }
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
        DestroyTransformChildren();

        BoardController.GenerateBoardCells(xBoardSize / BoardController.GetBoardCount(),
            yBoardSize / BoardController.GetBoardCount(), cellPrefab);
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
}
